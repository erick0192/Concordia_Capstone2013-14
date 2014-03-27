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
  Loop_I2C();
  Loop_Camera();
}

void Loop_Camera()
{
  if(watchdog.canMove() == false)
  {
    if(camera1 != 0 && camera1->GetPan() != SERVO_PAN_STOP_COMMAND) camera1->Pan(SERVO_PAN_STOP_COMMAND);
    if(camera2 != 0 && camera2->GetPan() != SERVO_PAN_STOP_COMMAND) camera2->Pan(SERVO_PAN_STOP_COMMAND);
    if(camera3 != 0 && camera3->GetPan() != SERVO_PAN_STOP_COMMAND) camera3->Pan(SERVO_PAN_STOP_COMMAND);
    if(camera4 != 0 && camera4->GetPan() != SERVO_PAN_STOP_COMMAND) camera4->Pan(SERVO_PAN_STOP_COMMAND);
  }
}
