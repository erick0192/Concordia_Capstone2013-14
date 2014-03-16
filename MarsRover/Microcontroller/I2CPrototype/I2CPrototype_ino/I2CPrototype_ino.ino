#include <Wire.h>
#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3
#define MESSAGE_SIZE 15 //1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)

  int requestInterval = 3000; // request data every 3 seconds
  unsigned long int leftLastTimeDataReceived = 0;
  unsigned long int rightLastTimeDataReceived = 0;
  unsigned long int time = 0;
  
  //For serial data
  char incomingByte = 0;
  String commandString = "";
  String leftMovementCommand = "";
  String rightMovementCommand = "";
  boolean commandStarted = false;
  char commandArr[20];
  
  //For I2C data
  String rightMotorStatus[3];
  String leftMotorStatus[3];
  char messageFromSlave[20];

void setup()
{
  Wire.begin();        // join i2c bus (address optional for master)
  Serial.begin(115200);  // start serial for output
}

void loop()
{  
  while (Serial.available() > 0) {
                // parse the incoming command
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
                      
                      //Serial.print("Left movement command received: ");
                      //Serial.println(leftMovementCommand);
                      
                      Wire.beginTransmission(LEFT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
                    }
                  
                  if(commandString[1] == 'R')
                    {
                      rightMovementCommand = commandString.substring(2, commandString.indexOf('>'));
                      rightMovementCommand.toCharArray(commandArr, leftMovementCommand.length());
                      
                      //Serial.print("Right movement command received: ");
                      //Serial.println(rightMovementCommand);
                      
                      Wire.beginTransmission(RIGHT_DEVICE_ADDRESS);
                      Wire.write(commandArr);
                      Wire.endTransmission();
                    }
                    
                  commandStarted = false;
                  commandString = "";
                }
  }
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

  delay(500);
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
