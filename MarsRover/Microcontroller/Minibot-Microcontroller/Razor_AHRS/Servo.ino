//Servo Controls
#include <Servo.h>

#define SERVO_YAW_PIN 9
#define SERVO_PITCH_PIN 10
#define SERVO_ROLL_PIN 11
#define UPDATE_SERVO_INTERVAL 50

Servo yawServo;
Servo pitchServo;
Servo rollServo;
int yawMicrosec;
int pitchMicrosec;
int rollMicrosec;
int stateTest = 1;
unsigned long servoUpdateTiming = millis();

void Servo_Init()
{
  yawServo.attach(SERVO_YAW_PIN);
  pitchServo.attach(SERVO_PITCH_PIN);
  rollServo.attach(SERVO_ROLL_PIN);
  yawServo.writeMicroseconds(1437);
  pitchServo.writeMicroseconds(1437);
  rollServo.writeMicroseconds(1437);
}

void Update_Servos()
{
   if((millis() - servoUpdateTiming) >= UPDATE_SERVO_INTERVAL)
   {
    servoUpdateTiming = millis();
    /*
    if (yaw < 0) yawMicrosec = 2100-(662*TO_DEG(yaw))/-179;
    if (yaw >= 0) yawMicrosec = 775+(662*TO_DEG(yaw))/179;
    if (pitch < 0) pitchMicrosec = 2100-(662*TO_DEG(pitch))/-90;
    if (pitch >= 0) pitchMicrosec = 775+(662*TO_DEG(pitch))/90;
    if (roll < 0) rollMicrosec = 2100-(662*TO_DEG(roll))/-179;
    if (roll >= 0) rollMicrosec = 775+(662*TO_DEG(roll))/179;
    */
    
    if (yaw < 0) yawMicrosec = map(TO_DEG(yaw),-90, -179,1437,2100);
    if (yaw >= 0) yawMicrosec = map(TO_DEG(yaw),179, 90,1437,775);
    if (pitch < 0) pitchMicrosec = map(TO_DEG(pitch),0, -90,1437,775);
    if (pitch >= 0) pitchMicrosec = map(TO_DEG(pitch),0, 90,1437,2100);
    if (roll < 0) rollMicrosec = map(TO_DEG(roll),0, -90,1437,775);
    if (roll >= 0) rollMicrosec = map(TO_DEG(roll),0, 90,1437,2100);
    
    yawServo.writeMicroseconds(yawMicrosec);
    pitchServo.writeMicroseconds(pitchMicrosec);
    rollServo.writeMicroseconds(rollMicrosec);
/*    
    yawMicrosec = 1500;
    yawServo.writeMicroseconds(yawMicrosec);
    if(yawMicrosec == 1500)
    {
      
    }*/
      /*
        if(stateTest == 1)
        {
        yawServo.writeMicroseconds(900);
        yawServo.writeMicroseconds(900);
        yawServo.writeMicroseconds(900);
        stateTest = 2;
        }
        if(stateTest == 2)
        {
        yawServo.writeMicroseconds(1200);
        yawServo.writeMicroseconds(1200);
        yawServo.writeMicroseconds(1200);
        stateTest = 1;
        }
        servoUpdateTiming = millis();
        */
        servoUpdateTiming = millis();
   }
      Serial.println(yawMicrosec);
      Serial.println(pitchMicrosec);
      Serial.println(rollMicrosec);
   /*
   Serial.println("SERVO UPDATING...");
   Serial.println(millis() - servoUpdateTiming);
   */
}
