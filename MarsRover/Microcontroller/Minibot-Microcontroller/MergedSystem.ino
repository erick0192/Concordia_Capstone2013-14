#include <SoftwareSerial.h>
#include "Globals.h"
#include "Camera.h"
#include "IMU.h"
#include <TinyGPS.h>
TinyGPS gps;


Camera *camera;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(OUTPUT_BAUD_RATE);
  Init_GPS();
  camera = new Camera(SERVO_CAM_1_PAN_PIN, SERVO_CAM_1_TILT_PIN, SERVO_CAM_1_ID);
  Init_IMU();
  Init_Wheels();
}

void loop() {
  Loop_GPS();
  Loop_IMU();
}




