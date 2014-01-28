#include <SoftwareSerial.h>
#include "IOPins.h"
#include "Camera.h"
#include "IMU.h"

/*
#define NUM_OF_WHEELS_PER_SIDE 2

Wheel TopRightWheel(TOP_RIGHT_WHEEL_ENABLE_PIN, TOP_RIGHT_WHEEL_FRONT_PIN, TOP_RIGHT_WHEEL_BACK_PIN); //Enable pin, front pin, back pin
Wheel TopLeftWheel(TOP_LEFT_WHEEL_ENABLE_PIN, TOP_LEFT_WHEEL_FRONT_PIN, TOP_LEFT_WHEEL_BACK_PIN);
Wheel BottomRightWheel(BOTTOM_RIGHT_WHEEL_ENABLE_PIN, BOTTOM_RIGHT_WHEEL_FRONT_PIN, BOTTOM_RIGHT_WHEEL_BACK_PIN);
Wheel BottomLeftWheel(BOTTOM_LEFT_WHEEL_ENABLE_PIN, BOTTOM_LEFT_WHEEL_FRONT_PIN, BOTTOM_LEFT_WHEEL_BACK_PIN);


Wheel rightWheels[] = {TopRightWheel, BottomRightWheel};
Wheel leftWheels[] = {TopLeftWheel, BottomLeftWheel}; 
*/

Camera camera(SERVO_CAM_PAN_PIN, SERVO_CAM_TILT_PIN);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(OUTPUT__BAUD_RATE);
  Init_GPS();
  //Init_Wheels();
  Init_IMU();
}
void loop() {
  // put your main code here, to run repeatedly: 
  Loop_GPS();
  //Loop_Wheels();
  Loop_IMU();
  Read_Serial();
}


