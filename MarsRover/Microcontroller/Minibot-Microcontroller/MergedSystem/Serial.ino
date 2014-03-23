#include <Wire.h>
#define MESSAGE_SIZE 15 //1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)

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
  
  //For I2C data
  String rightMotorStatus[3];
  String leftMotorStatus[3];
  char messageFromSlave[20];
  
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
                      
                      //Commented code only used for debugging so as to not saturate serial port for no reason
                      Serial.print("Pan camera ");
                      Serial.print(idCamera);
                      Serial.print(" with speed:");
                      Serial.println(angle); 
                      if(idCamera == '1' && camera1 != 0) camera1->Pan(angle);
                      else if(idCamera == '2' && camera2 != 0) camera2->Pan(angle);
                      else if(idCamera == '3' && camera3 != 0) camera3->Pan(angle);
                      else if(idCamera == '4' && camera4 != 0) camera4->Pan(angle);
                    }
                  
                  if(commandString[1] == CommandMetadata::SERVO_TILT)
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();

                      Serial.print("Tilt camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle);
                      if(idCamera == '1' && camera1 != 0) camera1->Tilt(angle);
                      else if(idCamera == '2' && camera2 != 0) camera2->Tilt(angle);
                      else if(idCamera == '3' && camera3 != 0) camera3->Tilt(angle);
                      else if(idCamera == '4' && camera4 != 0) camera4->Tilt(angle);
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
}

void Loop_I2C()
{
    time = millis();
  
  if ((time - leftLastTimeDataReceived) >= requestInterval){
    Wire.requestFrom(LEFT_DEVICE_ADDRESS, MESSAGE_SIZE);
    int counter = 0;
    while(Wire.available())
    {
      byte input = Wire.read();
      
      unsigned int inputEquiv = word(0,input);
     // Serial.print(inputEquiv);
     // Serial.print(" ");
      messageFromSlave[counter] = input;
      counter++;
    }
      messageFromSlave[counter] = '/0';
    
    //Serial.println();
    //Serial.println(messageFromSlave);
    CreateMovementCommand(messageFromSlave, leftMotorStatus, 'L');
    
    Serial.print(leftMotorStatus[0]);
    Serial.print(leftMotorStatus[1]);
    Serial.print(leftMotorStatus[2]);
    
    leftLastTimeDataReceived = millis();
  }
  
  if ((time - rightLastTimeDataReceived) >= requestInterval){
    Wire.requestFrom(RIGHT_DEVICE_ADDRESS, MESSAGE_SIZE);
    int counter = 0;
    
    while(Wire.available())
    {
      byte input = Wire.read();
       // unsigned int inputEquiv = word(0,input);
       // Serial.print(inputEquiv);
       // Serial.print(" ");
      messageFromSlave[counter] = input;
      counter++;
    }
    
    messageFromSlave[counter] = '/0';
    
    //Serial.println();
    //Serial.println(messageFromSlave);
    CreateMovementCommand(messageFromSlave, rightMotorStatus, 'R');
    Serial.print(rightMotorStatus[0]);
    Serial.print(rightMotorStatus[1]);
    Serial.print(rightMotorStatus[2]);

    rightLastTimeDataReceived = millis();
  }
}

float getCurrent(char c)
{
  unsigned int equivalentIntegerValue = word(0,c);
  float current = 0;
  const float CURRENT_CONVERSION_FACTOR = 0.0392;
  //temporary test conversion equation
  
  current = equivalentIntegerValue * CURRENT_CONVERSION_FACTOR;
  
  return current;
}


float getTemperature(char c)
{
   unsigned int equivalentIntegerValue = word(0,c);
  float temperature = 0;
  
  //temporary conversion factor
  const float TEMPERATURE_CONVERSION_FACTOR = 0.3921;
  
  temperature = equivalentIntegerValue * TEMPERATURE_CONVERSION_FACTOR;
  
  return temperature;
}

float calculateAverage(float temperatureArray[], int numberOfElements)
{
  float average = 0;
  
  for (int i = 0; i < numberOfElements; i++){
    average += temperatureArray[i];
  }
  average = average / float(numberOfElements);
  
  return average;
}

void CreateMovementCommand(char* messageFromSlave, String *motorCommandArray, char motorSide)
{
  float current1 = getCurrent(messageFromSlave[1]);
  float current2 = getCurrent(messageFromSlave[6]);
  float current3 = getCurrent(messageFromSlave[11]);
  
  float motor1Temperature[3];
  float motor2Temperature[3];
  float motor3Temperature[3];
  
  for(int i = 0; i < 3; i++)
  {
    motor1Temperature[i] = getTemperature(messageFromSlave[2+i]);
    motor2Temperature[i] = getTemperature(messageFromSlave[7+i]);
    motor3Temperature[i] = getTemperature(messageFromSlave[12+i]);
  }
  
  float motor1AverageTemperature = calculateAverage(motor1Temperature, 3);
  float motor2AverageTemperature = calculateAverage(motor2Temperature, 3);
  float motor3AverageTemperature = calculateAverage(motor3Temperature, 3);
  
  char tempFloatConversion[10]; //storage for floating point numbers
  
  if (motorSide == 'L')
  {
   motorCommandArray[0] = "MR;F," + String(motorSide) + "," + String(dtostrf(current1,3,2,tempFloatConversion)) + "," + String(dtostrf(motor1AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
   motorCommandArray[1] = "MR;M," + String(motorSide) + "," + String(dtostrf(current2,3,2,tempFloatConversion)) + "," + String(dtostrf(motor2AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
   motorCommandArray[2] = "MR;B," + String(motorSide) + "," + String(dtostrf(current3,3,2,tempFloatConversion)) + "," + String(dtostrf(motor3AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
  }
  else if (motorSide == 'R')
  {
   motorCommandArray[0] = "MR;F," + String(motorSide) + "," + String(dtostrf(current1,3,2,tempFloatConversion)) + "," + String(dtostrf(motor1AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
   motorCommandArray[1] = "MR;M," + String(motorSide) + "," + String(dtostrf(current2,3,2,tempFloatConversion)) + "," + String(dtostrf(motor2AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
   motorCommandArray[2] = "MR;B," + String(motorSide) + "," + String(dtostrf(current3,3,2,tempFloatConversion)) + "," + String(dtostrf(motor3AverageTemperature,3,2,tempFloatConversion)) + "" + "|";
  }
  return;
}
