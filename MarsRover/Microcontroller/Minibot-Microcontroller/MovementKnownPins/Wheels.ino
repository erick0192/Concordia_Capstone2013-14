//Author: Matthias Martineau
//Version: 1.0
//This version of the code is made for the minibot.
//For the actual version of the code, this board should
//instead relay its movement commands to the MSP430s
//over I2C instead of PWMing the motor controller directly.

#define NUM_OF_WHEELS_PER_SIDE 2

#define TOP_RIGHT_WHEEL_ENABLE_PIN 4
#define TOP_RIGHT_WHEEL_FRONT_PIN 49
#define TOP_RIGHT_WHEEL_BACK_PIN 47

#define TOP_LEFT_WHEEL_ENABLE_PIN 5
#define TOP_LEFT_WHEEL_FRONT_PIN 53
#define TOP_LEFT_WHEEL_BACK_PIN 55

#define BOTTOM_RIGHT_WHEEL_ENABLE_PIN 6
#define BOTTOM_RIGHT_WHEEL_FRONT_PIN 35
#define BOTTOM_RIGHT_WHEEL_BACK_PIN 37

#define BOTTOM_LEFT_WHEEL_ENABLE_PIN 7
#define BOTTOM_LEFT_WHEEL_FRONT_PIN 41
#define BOTTOM_LEFT_WHEEL_BACK_PIN 43


#include "Wheel.h"
#include "CommandMetadata.h"

void moveForward(int turn, int duty){
    analogWrite(TOP_RIGHT_WHEEL_FRONT_PIN, duty);
    analogWrite(TOP_RIGHT_WHEEL_BACK_PIN, 0);
    analogWrite(TOP_LEFT_WHEEL_FRONT_PIN, duty);
    analogWrite(TOP_LEFT_WHEEL_BACK_PIN, 0);
    analogWrite(BOTTOM_RIGHT_WHEEL_FRONT_PIN, duty);
    analogWrite(BOTTOM_RIGHT_WHEEL_BACK_PIN, 0);
    analogWrite(BOTTOM_LEFT_WHEEL_FRONT_PIN, duty);
    analogWrite(BOTTOM_LEFT_WHEEL_BACK_PIN, 0);
}

void moveBackward(int turn, int duty){
    analogWrite(TOP_RIGHT_WHEEL_FRONT_PIN, 0);
    analogWrite(TOP_RIGHT_WHEEL_BACK_PIN, duty);
    analogWrite(TOP_LEFT_WHEEL_FRONT_PIN, 0);
    analogWrite(TOP_LEFT_WHEEL_BACK_PIN, duty);
    analogWrite(BOTTOM_RIGHT_WHEEL_FRONT_PIN, 0);
    analogWrite(BOTTOM_RIGHT_WHEEL_BACK_PIN, duty);
    analogWrite(BOTTOM_LEFT_WHEEL_FRONT_PIN, 0);
    analogWrite(BOTTOM_LEFT_WHEEL_BACK_PIN, duty);
}

void ParseCommand(char *command){
     // Serial.println(command);
     char directionLeft = command[1];
     char directionRight = command[5];
     
     char leftSpeedStr[4], rightSpeedStr[4];
     
     for(int i = 0; i < 3; i++){
       leftSpeedStr[i] = command[i+2];
       rightSpeedStr[i] = command[i+6];
     }
     
     leftSpeedStr[3] = '\0';
     rightSpeedStr[3] = '\0';
   
     int speedLeft = atoi(leftSpeedStr);
     int speedRight = atoi(rightSpeedStr);
     
     
     //If we received a value outside of the acceptable range, something probably went wrong. Don't do anything.
     if (speedLeft < 0 || speedLeft > 255){
//       Serial.print("Invalid speed value received: ");
//       Serial.println(speedLeft);
       return;
     }
     
     if (speedRight < 0 || speedRight > 255){
//       Serial.print("Invalid speed value received: ");
//       Serial.println(speedLeft);
       return;
     }
     
     if (directionLeft == CommandMetadata::MOVE_FORWARD){
       moveForward(leftWheels, speedLeft);
     }
     else if (directionLeft == CommandMetadata::MOVE_BACKWARD){
       moveBackward(leftWheels, speedLeft);
     }
     else{
    //   Serial.println("Error understanding left-side command");
     }
     
     if (directionRight == CommandMetadata::MOVE_FORWARD){
       moveForward(rightWheels, speedRight);
     }
     else if (directionRight == CommandMetadata::MOVE_BACKWARD){
       moveBackward(rightWheels, speedRight);
     }
     else{
  //     Serial.println("Error understanding right-side command");
     }
     
}

void Init_Wheels() {
  Serial.begin(9600);
  
  for(int i = 0; i < NUM_OF_WHEELS_PER_SIDE; i++){
  //Setup enable pins on both the left and right sides of the rover
    pinMode(leftWheels[i].getEnablePin(), OUTPUT);
    pinMode(rightWheels[i].getEnablePin(), OUTPUT);
    
    //Turn on the enable
    digitalWrite(leftWheels[i].getEnablePin(), HIGH); 
    digitalWrite(rightWheels[i].getEnablePin(), HIGH);
    
    //Setup movement pins
    pinMode(leftWheels[i].getFrontPin(), OUTPUT);
    pinMode(leftWheels[i].getBackPin(), OUTPUT);
    pinMode(rightWheels[i].getFrontPin(), OUTPUT);
    pinMode(rightWheels[i].getBackPin(), OUTPUT); 
  }
}

void Loop_Wheels() {
  
//Command processing
char charRead, command[CommandMetadata::COMMAND_SIZE +1]; //+1 for null terminator
int index = 0;
unsigned long timeoutInterval = 100; //100ms timeout period 
unsigned long timeStart = 0; //Used to keep track of timeout 
unsigned long timeNow = 0;

if(Serial.available() >= 1){
  timeStart = millis();
  
  while(Serial.available() >= 1){  
    while (Serial.peek() != CommandMetadata::COMMAND_START){ //Flush garbage until command start has been received
        charRead = Serial.read();
    }
      
    if(Serial.available() >= CommandMetadata::COMMAND_SIZE){ //A potential command has arrived. Read and parse it.
     index = 0;
      /*
      Things to check for:
      String received is greater than command length (NOT DONE YET)
      */
      
      charRead = Serial.read(); //Read in the command start character
      
      while (Serial.peek() != CommandMetadata::COMMAND_END){

        command[index] = Serial.read();
        index++;
      }
      command[index] = '\0'; //Insert null termination character
      
      charRead = Serial.read(); //Read in command end character
      
      ParseCommand(command);
    }
    else{ //Keep track of the amount of time which has passed since the first character has arrived.
      timeNow = millis();
      
      if (timeNow - timeStart >= timeoutInterval){ //A full command has not been received in time. Flush the buffer
        Serial.println("Timed out");
        Serial.flush();
      }
    }
  }
}

}
