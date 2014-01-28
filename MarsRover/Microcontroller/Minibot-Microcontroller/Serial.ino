static const int DELAY_IMU = 1000; //Delay to update the IMU
unsigned long startIMU = 0; //Keeping track of when IMU started counting

static const int DELAY_GPS = 1000; //Delay to update the IMU
unsigned long startGPS = 0; //Keeping track of when IMU started counting

void Send_IMU(float yaw, float pitch, float roll)
{
  if(startIMU == 0)
  {
    startIMU = millis();
  }
  if(millis() - startIMU > DELAY_IMU)
  {
    Serial.print("I");
    Serial.print(TO_DEG(yaw)); Serial.print(",");
    Serial.print(TO_DEG(pitch)); Serial.print(",");
    Serial.print(TO_DEG(roll)); Serial.print("|");
    Serial.println();
    startIMU = millis();
  }
}

void Send_GPS(float latitude, float longitude, float alt)
{
    if(startGPS == 0)
  {
    startGPS = millis();
  }
  if(millis() - startGPS > DELAY_GPS)
  {
    Serial.print("G");
    Serial.print(latitude); Serial.print(",");
    Serial.print(longitude); Serial.print(",");
    Serial.print(alt); Serial.print("|");
    Serial.println();
    startIMU = millis();
  }
}

char incomingByte = 0;   // for incoming serial data
String commandString = "";
boolean commandStarted = false;
int angle = 0;
char idCamera = 0;

void Read_Serial()
{
          // send data only when you receive data:
        if (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read();
                if(incomingByte == '<' || commandStarted == true)
                {
                  commandStarted = true;
                  commandString = commandString + incomingByte;
                }
                if(incomingByte == '>')
                {
                  if(commandString[1] == 'P')
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();

                      Serial.print("Pan camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle);
                      camera.Pan(angle);
                    }
                  
                  if(commandString[1] == 'T')
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();

                      Serial.print("Tilt camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle);
                      camera.Tilt(angle);
                    }
                    
                  commandStarted = false;
                  commandString = "";
                }
        }
  /*
    if(Serial.available())
  {
  char *p = sz;
  char *str;
  Serial.begin(9600);
  while ((str = strtok_r(p, ";", &p)) != NULL) // delimiter is the semicolon
    Serial.println(str);
  }
  */
}
