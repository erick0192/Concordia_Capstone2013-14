//Left Slave Code
#include <Wire.h>
#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3
#define SPEED_INCREMENT 5;

short int targetSpeed[3];
short int movementSpeed[3];
String commandString = "";
char incomingByte = 0;
boolean commandStarted = false;
int incomingMovement1 = 0;
int incomingMovement2 = 0;
int incomingMovement3 = 0;

//1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)

void setup()
{
  Wire.begin(LEFT_DEVICE_ADDRESS);        // join i2c bus (address optional for master)
  Wire.onRequest(reportStatusEvent);
  Wire.onReceive(receiveCommandEvent);
  Serial.begin(9600);  // start serial for output
  
  targetSpeed[0] = 0;
  targetSpeed[1] = 0;
  targetSpeed[2] = 0;
  movementSpeed[0] = 0;
  movementSpeed[1] = 0;
  movementSpeed[2] = 0;
}

void loop()
{
  int speedDifference = targetSpeed[0] - movementSpeed[0];
  if(speedDifference != 0)
  {
    Serial.print("Motor 1 Target: ");
    Serial.println(targetSpeed[0]);
    Serial.print("Motor 1 Speed: ");
    Serial.println(movementSpeed[0]);
    
    if (speedDifference > 5){
      movementSpeed[0] += SPEED_INCREMENT;
    }
    else if(speedDifference < -5 ){
      movementSpeed[0] -= SPEED_INCREMENT;
    }
    else{
      movementSpeed[0] += speedDifference; 
    } 
  }
  
  speedDifference = targetSpeed[1] - movementSpeed[1];
  if(speedDifference != 0)
  {
  //  Serial.print("Motor 2 Target: ");
  //  Serial.println(targetSpeed[1]);
  //  Serial.print("Motor 2 Speed: ");
  //  Serial.println(movementSpeed[1]);
    
    if (speedDifference > 5){
      movementSpeed[1] += SPEED_INCREMENT;
    }
    else if(speedDifference < -5 ){
      movementSpeed[1] -= SPEED_INCREMENT;
    }
    else{
      movementSpeed[1] += speedDifference;
    }
  }
  
  speedDifference = targetSpeed[2] - movementSpeed[2];
  if(speedDifference != 0)
  {
  //  Serial.print("Motor 3 Target: ");
  //  Serial.println(targetSpeed[2]);
  //  Serial.print("Motor 3 Speed: ");
  //  Serial.println(movementSpeed[2]);
    
    if (speedDifference > 5){
      movementSpeed[2] += SPEED_INCREMENT;
    }
    else if(speedDifference < -5 ){
      movementSpeed[2] -= SPEED_INCREMENT;
    }
    else{
      movementSpeed[2] += speedDifference;
    }
  }
  
 // Serial.print(movementSpeed[0]);
 // Serial.print(movementSpeed[1]);
 // Serial.println(movementSpeed[2]);
  
 // Serial.print(targetSpeed[0]);
 // Serial.print(targetSpeed[1]);
 // Serial.println(targetSpeed[2]);
  delay(50);
}

void reportStatusEvent()
{
  Wire.write("YYY YYY YYY YYY"); //send 15 characters as expected
}

void receiveCommandEvent(int numOfBytesReadFromMaster)
{
   while(Wire.available()) // read F255F255F255
  {
    incomingByte = Wire.read(); // receive byte as a character
    commandString = commandString + incomingByte;
          // print the character
  }
  incomingMovement1 = commandString.substring(1,4).toInt();
  incomingMovement2 = commandString.substring(5,8).toInt();
  incomingMovement3 = commandString.substring(9,12).toInt();
  noInterrupts();
  targetSpeed[0] = incomingMovement1;
  targetSpeed[1] = incomingMovement2;
  targetSpeed[2] = incomingMovement3;
  interrupts();
  Serial.println(targetSpeed[0]);
  commandString = "";
}
