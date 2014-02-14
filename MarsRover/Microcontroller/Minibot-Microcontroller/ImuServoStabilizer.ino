/*
This class deals with the IMU and Servo interactions
enabling the servos to act as stabilizers or replicators of movement ( arm )
*/
#include <Servo.h>
#define UPDATE_SERVO_INTERVAL 50

Servo yawServo;
Servo pitchServo;
Servo rollServo;

int yawMicrosec;
int pitchMicrosec;
int rollMicrosec;
int stateTest = 1;
int servoArmState = 2; // 1:Normal Arm Mode 2: Stabilization Mode

unsigned long servoUpdateTiming = millis();

void Servo_Init()
{
  yawServo.attach(SERVO_STABILIZER_YAW_PIN);
  pitchServo.attach(SERVO_STABILIZER_PITCH_PIN);
  rollServo.attach(SERVO_STABILIZER_ROLL_PIN);
  
  yawServo.writeMicroseconds(1437);
  pitchServo.writeMicroseconds(1437);
  rollServo.writeMicroseconds(1437);
}

void Update_Servos()
{
   if((millis() - servoUpdateTiming) >= UPDATE_SERVO_INTERVAL)
   {
    servoUpdateTiming = millis();
    if(servoArmState == 1)
    {
    if (yaw < 0) yawMicrosec = map(TO_DEG(yaw),-90, -179,2100,1437);
    if (yaw >= 0) yawMicrosec = map(TO_DEG(yaw),179, 90,1437,775);
    if (pitch < 0) pitchMicrosec = map(TO_DEG(pitch),0, -90,1437,775);
    if (pitch >= 0) pitchMicrosec = map(TO_DEG(pitch),0, 90,1437,2100);
    if (roll < 0) rollMicrosec = map(TO_DEG(roll),0, -90,1437,775);
    if (roll >= 0) rollMicrosec = map(TO_DEG(roll),0, 90,1437,2100);
    }
    else if(servoArmState == 2)
   {
    if (yaw < 0) yawMicrosec = map(TO_DEG(yaw),-90, -179,775,1437);
    if (yaw >= 0) yawMicrosec = map(TO_DEG(yaw),179,90,1437,2100);
    if (pitch < 0) pitchMicrosec = map(TO_DEG(pitch),0, -90,1437,2100);
    if (pitch >= 0) pitchMicrosec = map(TO_DEG(pitch),0, 90,1437,775);
    if (roll < 0) rollMicrosec = map(TO_DEG(roll),0, -90,1437,2100);
    if (roll >= 0) rollMicrosec = map(TO_DEG(roll),0, 90,1437,775);
   } 
    yawServo.writeMicroseconds(yawMicrosec);
    pitchServo.writeMicroseconds(pitchMicrosec);
    rollServo.writeMicroseconds(rollMicrosec);
    servoUpdateTiming = millis();
   }
}


