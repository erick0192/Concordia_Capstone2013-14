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
                      if(idCamera == '1') camera->Pan(angle);
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
                      if(idCamera == '1') camera->Tilt(angle);
                    }
                    
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
                    }                    
                  commandStarted = false;
                  commandString = "";
                }
        }
}



