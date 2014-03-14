#include <SoftwareSerial.h>
#include "Globals.h"
#include "Camera.h"
#include "IMU.h"
#include <TinyGPS.h>
#include "Watchdog.h"
TinyGPS gps;


Camera *camera1;
Camera *camera2;
Camera *camera3;
Camera *camera4;

void setup() {
  // put your setup code here, to run once:
  Wire.begin();
  Serial.begin(OUTPUT_BAUD_RATE);
  watchdog.initialize(3000000);
  Init_GPS();
  Init_IMU();
  Init_Wheels();
  
  camera1 = new Camera(SERVO_CAM_1_PAN_PIN, SERVO_CAM_1_TILT_PIN, SERVO_CAM_1_ID);
  camera2 = new Camera(SERVO_CAM_2_PAN_PIN, SERVO_CAM_2_TILT_PIN, SERVO_CAM_2_ID);
  camera3 = new Camera(SERVO_CAM_3_PAN_PIN, SERVO_CAM_3_TILT_PIN, SERVO_CAM_3_ID);
  camera4 = new Camera(SERVO_CAM_4_PAN_PIN, SERVO_CAM_4_TILT_PIN, SERVO_CAM_4_ID);
}

void loop() {
  Loop_GPS();
  Loop_IMU();
}




