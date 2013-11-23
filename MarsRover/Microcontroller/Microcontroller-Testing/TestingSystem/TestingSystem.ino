//Author: Matthias Martineau
//Version: 1.0
//This version of the code is made for the minibot.
//For the actual version of the code, this board should
//instead relay its movement commands to the MSP430s
//over I2C instead of PWMing the motor controller directly.

#define NUM_OF_WHEELS_PER_SIDE 2
#define NUM_OF_SERVOS 2

#include "Wheel.h"
#include "CommandMetadata.h"
#include <Servo.h>
/*-------------------WHEEL RELATED VARIABLES AND FUNCTIONS-------------------*/

Wheel TopRightWheel(23, 4, 5); //Enable pin, front pin, back pin
Wheel TopLeftWheel(22, 10, 11);
Wheel BottomRightWheel(24, 7, 6);
Wheel BottomLeftWheel(25, 8, 9);

Wheel rightWheels[] = {TopRightWheel, BottomRightWheel};
Wheel leftWheels[] = {TopLeftWheel, BottomLeftWheel};

void moveForward(Wheel sideWheels[], int duty){
  for (int i = 0; i < NUM_OF_WHEELS_PER_SIDE; i++){
    analogWrite(sideWheels[i].getFrontPin(), duty);
    analogWrite(sideWheels[i].getBackPin(), 0);
  }
}

void moveBackward(Wheel sideWheels[], int duty){
  for (int i = 0; i < NUM_OF_WHEELS_PER_SIDE; i++){
    analogWrite(sideWheels[i].getFrontPin(), 0);
    analogWrite(sideWheels[i].getBackPin(), duty);
  }
}

/*-------------------SERVO RELATED VARIABLES AND FUNCTIONS-------------------*/

Servo boomPan;  //Servo for panning camera on boom
Servo boomTilt;   //Servo for tilting camera on boom
int servoPositions[NUM_OF_SERVOS]; //Keeps track of the positions of all servos controlled



/*-------------------COMMAND PARSING-------------------*/
void parseCommand(char *command){
  
     if(command[0] == "M"){
     }
     else if(command[0] == "S"){
     }
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

boolean readCommand(char* command){
  char charRead;
  int index = 0;
  
  while(serial.available()){
    charRead = serial.read();
    
    if (charRead == CommandMetadata::COMMAND_END){
      command[index] = '/0';
      return true;
    }
    
    command[index] = serial.read();
    index++;
    
    if(index > CommandMetadata::MAX_COMMAND_SIZE + 1) // If the index is beyond the point where the additional null termination character can be inserted then there is a problem
    return false;
  }
  
  return false; //There is no more data and yet the command ending character has not been received yet
}

void setup() {
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
  
  boomPan.attach(12);
  boomTilt.attach(13, 780, 2380);
}

void loop() {
  
  //Command processing
  char charRead, command[CommandMetadata::MAX_COMMAND_SIZE +1]; //+1 for null terminator

  unsigned long timeoutInterval = 100; //100ms timeout period 
  unsigned long timeStart = 0; //Used to keep track of timeout 
  unsigned long timeNow = 0;

  if(Serial.available() >= 1){
    timeStart = millis();
  
  
    while(Serial.peek() != CommandMetadata::COMMAND_START && Serial.available() >= 1){
      charRead = Serial.read();
    }
  
    if(readCommand(*command)){
      Serial.println(command);
     // parseCommand(command);
    }
      
  }
}
      /*
    if(Serial.available() >= CommandMetadata::COMMAND_SIZE){ //A potential command has arrived. Read and parse it.
     index = 0;
      
      //Things to check for:
     // String received is greater than command length (NOT DONE YET)
      
      
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
    */
