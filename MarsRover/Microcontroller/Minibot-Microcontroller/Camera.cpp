#include "Arduino.h"
#include "Camera.h"
#include "Globals.h"
int Camera::numCam = 0;

Camera::Camera(int pinServoPan, int pinServoTilt,int camID)
{
  Camera::numCam++;
  cameraID = camID;
  _pinServoPan = pinServoPan;
  _pinServoTilt = pinServoTilt;
  _servoPan.attach(_pinServoPan);
  _servoTilt.attach(_pinServoTilt);
  Pan(1521);
  Tilt(1500);
}

//Note that sending a 90 degree angle to the 360 continuous panning servo make it stop
void Camera::Pan(int angle)
{
  //Stop limit 1465 to 1577
  //Full stop 1521
  //_servoPan.writeMicroseconds((int)map(angle,0, 90,993,1119));
  _servoPan.writeMicroseconds(angle);
}

void Camera::Tilt(int angle)
{
  //_servoTilt.writeMicroseconds((int)map(angle,0, 90,800,2200));
  _servoTilt.writeMicroseconds(angle);
}

