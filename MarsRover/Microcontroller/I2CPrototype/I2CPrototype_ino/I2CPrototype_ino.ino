#include <Wire.h>
#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3
#define COMMAND_SIZE 15 //1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)

  /*
  if after 5 seconds we receive nothing from the microcontrollers
  then we should send a reset signal.
  */
  int timeout = 5000;
  int requestInterval = 3000; // request data every 3 seconds
  unsigned long int leftLastTimeDataReceived = 0;
  unsigned long int rightLastTimeDataReceived = 0;
  unsigned long int time = 0;
  char incomingByte = 0;   // for incoming serial data
  String commandString = "";
  String leftMovementCommand = "";
  String rightMovementCommand = "";
  boolean commandStarted = false;
  char commandArr[20];

void setup()
{
  Wire.begin();        // join i2c bus (address optional for master)
  Serial.begin(9600);  // start serial for output

}

void loop()
{  
  while (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read();
                if(incomingByte == '<' || commandStarted == true)
                {
                  commandStarted = true;
                  commandString = commandString + incomingByte;
                }
                if(incomingByte == '>')
                {
                  if(commandString[1] == 'L')
                    {
                      leftMovementCommand = commandString.substring(2, commandString.indexOf('>'));
                      leftMovementCommand.toCharArray(commandArr, leftMovementCommand.length() + 1);
                      
                      Serial.print("Left movement command received: ");
                      Serial.println(leftMovementCommand);
                      
                      Wire.beginTransmission(LEFT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
                    }
                  
                  if(commandString[1] == 'R')
                    {
                      rightMovementCommand = commandString.substring(2, commandString.indexOf('>'));
                      rightMovementCommand.toCharArray(commandArr, leftMovementCommand.length());
                      
                      Serial.print("Right movement command received: ");
                      Serial.println(rightMovementCommand);
                      
                      Wire.beginTransmission(RIGHT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
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
  
  //  if ((time - rightLastTimeDataReceived) >= requestInterval){
  //  Wire.requestFrom(RIGHT_DEVICE_ADDRESS, COMMAND_SIZE);
    
  // while(Wire.available())
   // {
  //    char input = Wire.read();
  //    Serial.print(input); // to be replaced
  //  }
    
  //  Serial.println();
   // rightLastTimeDataReceived = millis();
  //}
  
  //logic for setting reset pin should slaves become unresponsive?
  
  //Serial.println("I'm still alive!");
  delay(500);
}
