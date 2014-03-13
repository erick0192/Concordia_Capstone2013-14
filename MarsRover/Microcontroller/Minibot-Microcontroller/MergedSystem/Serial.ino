#include <Wire.h>
#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3
#define COMMAND_SIZE 15 //1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)


static const int DELAY_IMU = 1000; //Delay to update the IMU
unsigned long startIMU = 0; //Keeping track of when IMU started counting

static const int DELAY_GPS = 1000; //Delay to update the IMU
unsigned long startGPS = 0; //Keeping track of when IMU started counting

void Send_IMU(float yaw, float pitch, float roll)
{
    if(Serial.available() == 0)
  {
  if(startIMU == 0)
  {
    startIMU = millis();
  }
  if(millis() - startIMU > DELAY_IMU)
  {
    Serial.print("I;");
    Serial.print(TO_DEG(yaw)); Serial.print(",");
    Serial.print(TO_DEG(pitch)); Serial.print(",");
    Serial.print(TO_DEG(roll)); Serial.print("|");
    Serial.println();
    startIMU = millis();
  }
  }
}

static void print_float(float val, float invalid, int len, int prec)
{
  if (val == invalid)
  {
    while (len-- > 1){ Serial.print('0'); }
  }
  else
  {
    Serial.print(val, prec);
    int vi = abs((int)val);
    int flen = prec + (val < 0.0 ? 2 : 1); // . and -
    flen += vi >= 1000 ? 4 : vi >= 100 ? 3 : vi >= 10 ? 2 : 1;
    for (int i=flen; i<len; ++i)
      Serial.print(' ');
  }
}

void Send_GPS(float latitude, float longitude, float alt)
{
  if(Serial.available() == 0)
  {
    if(startGPS == 0)
  {
    startGPS = millis();
  }
  if(millis() - startGPS > DELAY_GPS)
  {
    Serial.print("G;");
    print_float(latitude, TinyGPS::GPS_INVALID_F_ANGLE, 10, 6);
    Serial.print(",");
    print_float(longitude, TinyGPS::GPS_INVALID_F_ANGLE, 11, 6); 
    Serial.print(",");
    Serial.print(alt); Serial.print("|");
    Serial.println();
    startGPS = millis();
  }
  }
}

  /*
  if after 5 seconds we receive nothing from the microcontrollers
  then we should send a reset signal.
  */
  int timeout = 5000;
  int requestInterval = 3000; // request data every 3 seconds
  unsigned long int leftLastTimeDataReceived = 0;
  unsigned long int rightLastTimeDataReceived = 0;
  unsigned long int time = 0;
  String leftMovementCommand = "";
  String rightMovementCommand = "";
  boolean commandStarted = false;
  char commandArr[20];

void serialEvent()
{
  char incomingByte = 0;   // for incoming serial data
  String commandString = "";
  boolean commandStarted = false;
  int angle = 0;
  int leftSpeed = 0;
  int rightSpeed = 0;
  char idCamera = 0;
          // send data only when you receive data:

        while (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read();
                if(incomingByte == CommandMetadata::COMMAND_START || commandStarted == true)
                {
                  commandStarted = true;
                  commandString = commandString + incomingByte;
                }
                if(incomingByte == CommandMetadata::COMMAND_END)
                {
                  if(commandString[1] == CommandMetadata::SERVO_PAN)
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();
                      
                      /* Commented code only used for debugging so as to not saturate serial port for no reason
                      Serial.print("Pan camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle); */
                      if(idCamera == '1') camera1->Pan(angle);
                      else if(idCamera == '2') camera2->Pan(angle);
                      else if(idCamera == '3') camera3->Pan(angle);
                      //else if(idCamera == '4') camera4->Pan(angle);
                    }
                  
                  if(commandString[1] == CommandMetadata::SERVO_TILT)
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();
/*
                      Serial.print("Tilt camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle); */
                      if(idCamera == '1') camera1->Tilt(angle);
                      else if(idCamera == '2') camera2->Tilt(angle);
                      else if(idCamera == '3') camera3->Tilt(angle);
                      //else if(idCamera == '4') camera4->Tilt(angle);
                    }
                    
                    if(commandString[1] == CommandMetadata::I2C_LEFT)
                    {
                      leftMovementCommand = commandString.substring(2, commandString.indexOf('>'));
                      leftMovementCommand.toCharArray(commandArr, leftMovementCommand.length() + 1);
                      
                      Serial.print("Left movement command received: ");
                      Serial.println(leftMovementCommand);
                      
                      Wire.beginTransmission(LEFT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
                    }
                  
                  if(commandString[1] == CommandMetadata::I2C_RIGHT)
                    {
                      rightMovementCommand = commandString.substring(2, commandString.indexOf('>'));
                      rightMovementCommand.toCharArray(commandArr, leftMovementCommand.length());
                      
                      Serial.print("Right movement command received: ");
                      Serial.println(rightMovementCommand);
                      
                      Wire.beginTransmission(RIGHT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
                    }
                    
                  /*  
                  if(commandString[1] == CommandMetadata::MOTOR_COMMAND)
                    {
                      leftSpeed = commandString.substring(3, 6).toInt();
                      rightSpeed = commandString.substring(7, 10).toInt();
                        if(commandString[2] == CommandMetadata::MOVE_FORWARD)
                      {
                        MoveForwardLeft(leftSpeed);
                       // Serial.println("Move Forward Left");
                      }
                        if(commandString[2] == CommandMetadata::MOVE_BACKWARD)
                      {
                        MoveBackwardLeft(leftSpeed);
                        //Serial.println("Move Backward Left");
                      }
                        if(commandString[6] == CommandMetadata::MOVE_FORWARD)
                      {
                        MoveForwardRight(leftSpeed);
                        //Serial.println("Move Forward Right");
                      }
                        if(commandString[6] == CommandMetadata::MOVE_BACKWARD)
                      {
                        MoveBackwardRight(leftSpeed);
                        //Serial.println("Move Forward Left");
                      } 
                      /*
                      Serial.print("Left Speed ");
                      Serial.print(leftSpeed);
                      Serial.print(" Right speed:");
                      Serial.println(rightSpeed);
                      */
                    //}
                    
                  if (commandString[1] == CommandMetadata::SERIAL_KEEP_ALIVE)
					{
					  watchdog.reportActivity();
					}
                  commandStarted = false;
                  commandString = "";
                }
        }
        
          
  //receive command
  time = millis();
  
  if ((time - leftLastTimeDataReceived) >= requestInterval){
    Wire.requestFrom(LEFT_DEVICE_ADDRESS, COMMAND_SIZE);
    
    while(Wire.available())
    {
      char input = Wire.read();
      Serial.print(input); // to be replaced
    }
    
    Serial.println();
    
    leftLastTimeDataReceived = millis();
  }
}



