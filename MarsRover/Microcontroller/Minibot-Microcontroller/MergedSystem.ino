#include <SoftwareSerial.h>
#include "Globals.h"
#include "Camera.h"
#include "IMU.h"
#include <TinyGPS.h>
TinyGPS gps;
/*
#define NUM_OF_WHEELS_PER_SIDE 2
 
 Wheel TopRightWheel(TOP_RIGHT_WHEEL_ENABLE_PIN, TOP_RIGHT_WHEEL_FRONT_PIN, TOP_RIGHT_WHEEL_BACK_PIN); //Enable pin, front pin, back pin
 Wheel TopLeftWheel(TOP_LEFT_WHEEL_ENABLE_PIN, TOP_LEFT_WHEEL_FRONT_PIN, TOP_LEFT_WHEEL_BACK_PIN);
 Wheel BottomRightWheel(BOTTOM_RIGHT_WHEEL_ENABLE_PIN, BOTTOM_RIGHT_WHEEL_FRONT_PIN, BOTTOM_RIGHT_WHEEL_BACK_PIN);
 Wheel BottomLeftWheel(BOTTOM_LEFT_WHEEL_ENABLE_PIN, BOTTOM_LEFT_WHEEL_FRONT_PIN, BOTTOM_LEFT_WHEEL_BACK_PIN);
 
 
 Wheel rightWheels[] = {TopRightWheel, BottomRightWheel};
 Wheel leftWheels[] = {TopLeftWheel, BottomLeftWheel}; 
 */

Camera *camera;
int incrementServo = 800;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(OUTPUT_BAUD_RATE);
  Init_GPS();
  //Init_Wheels();
  Init_IMU();
  camera = new Camera(SERVO_CAM_1_PAN_PIN, SERVO_CAM_1_TILT_PIN, SERVO_CAM_1_ID);
}
void loop() {
  /*
  for(int i = 800; i <= 2200; i=i+10)
   {
   delay(500);
   camera.Tilt(i);
   Serial.println(i);
   }
   for(int i = 2200; i >= 800; i=i-10)
   {
   delay(500);
   camera.Tilt(i);
   Serial.println(i);
   }  
   */
  /*
  while(1)
   {
   camera.Pan(0);
   camera.Tilt(0);
   delay(1000);
   camera.Pan(45);
   camera.Tilt(45);
   delay(1000);
   camera.Pan(90);
   camera.Tilt(90);
   delay(1000);
   } */
  // put your main code here, to run repeatedly: 
  Loop_GPS();
  //Loop_Wheels();
  Loop_IMU();
  //Read_Serial();
}




