#include "Arduino.h"
#include "Camera.h"
#include "IOPins.h"

Camera::Camera(int pinServoPan, int pinServoTilt)
{
  _pinServoPan = pinServoPan;
  _pinServoTilt = pinServoTilt;
  _servoPan.attach(SERVO_CAM_PAN_PIN);
  _servoTilt.attach(SERVO_CAM_TILT_PIN);
}

void Camera::Pan(int angle)
{
  _servoPan.write(angle);
}

void Camera::Tilt(int angle)
{
  _servoTilt.write(angle);
}
