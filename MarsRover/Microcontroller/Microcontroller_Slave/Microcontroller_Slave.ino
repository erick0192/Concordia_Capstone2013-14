//Left Slave Code
#include <Wire.h>
#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3

const int SPEED_INCREMENT = 5;
const int WHEEL_ONE = 9;
const int WHEEL_TWO = 10;
//const int WHEEL_THREE = 11;

int targetSpeed[3];
unsigned int movementSpeed[3];
String commandString = "";
char incomingByte = 0;
boolean commandStarted = false;
int incomingMovement1 = 0;
int incomingMovement2 = 0;
int incomingMovement3 = 0;

//1 byte for speed (per motor), 1 byte for current (per motor), 3 bytes for temperature (per motor)

void setup()
{
  Wire.begin(LEFT_DEVICE_ADDRESS); 
  //Wire.begin(RIGHT_DEVICE_ADDRESS);
  Wire.onRequest(reportStatusEvent);
  Wire.onReceive(receiveCommandEvent);
  Serial.begin(9600);  // start serial for output
  pinMode(WHEEL_ONE, OUTPUT);
  pinMode(WHEEL_TWO, OUTPUT);
  //pinMode(WHEEL_THREE, OUTPUT);
  
  targetSpeed[0] = 0;
  targetSpeed[1] = 0;
  targetSpeed[2] = 0;
  movementSpeed[0] = 0;
  movementSpeed[1] = 0;
  movementSpeed[2] = 0;
}

void loop()
{
  //Real code for the main loop to be implemented by the ECE team
  int speedDifference = targetSpeed[0] - movementSpeed[0];
  if(speedDifference != 0)
  {
   // Serial.print("Motor 1 Target: ");
   // Serial.println(targetSpeed[0]);
   // Serial.print("Motor 1 Speed: ");
    //Serial.println(movementSpeed[0]);
    
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
  
  Serial.print(movementSpeed[0]);
  Serial.print(" ");
  Serial.print(movementSpeed[1]);
  Serial.print(" ");
  Serial.println(movementSpeed[2]);
  
  analogWrite(WHEEL_ONE, movementSpeed[0]);
  analogWrite(WHEEL_TWO, movementSpeed[1]);

  delay(50);
}

int moveRover(int targetSpeed, int currentSpeed, int speedDifference, int maxIncrementalRate, char movementDirection)
{
      if (speedDifference > maxIncrementalRate){
      currentSpeed += maxIncrementalRate;
    }
    else if(speedDifference < -maxIncrementalRate ){
      currentSpeed -= maxIncrementalRate;
    }
    else{
      currentSpeed += speedDifference; 
    } 
    
    return currentSpeed;
}

void reportStatusEvent()
{
  //command format: SCTTTSCTTTSCTTT (speed, current, temp1 temp2 temp3 for motor 1, repeat for motors 2 and 3)
  
  //dummy current and temperature values. Up to ECE to make a proper implementation
  byte statusMessage[15];
  statusMessage[0] = movementSpeed[0];
  statusMessage[1] = 145;
  statusMessage[2] = 80;
  statusMessage[3] = 85;
  statusMessage[4] = 90;
  statusMessage[5] = movementSpeed[1];
  statusMessage[6] = 145;
  statusMessage[7] = 80;
  statusMessage[8] = 85;
  statusMessage[9] = 90;
  statusMessage[10] = movementSpeed[2];
  statusMessage[11] = 145;
  statusMessage[12] = 80;
  statusMessage[13] = 85;
  statusMessage[14] = 90;
  
 //Ensure 15 bytes are sent even if a null character is found
 //in statusMessage 
 Wire.write(statusMessage,15);

}

void receiveCommandEvent(int numOfBytesReadFromMaster)
{
   while(Wire.available()) // read F255F255F255
  {
    incomingByte = Wire.read(); // receive byte as a character
    commandString = commandString + incomingByte;
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
