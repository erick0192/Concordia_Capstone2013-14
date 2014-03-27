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
  Pan(100); // Stopping Pan
  Tilt(45); // Centering Tilt
}

//Note that sending a 90 degree angle to the 360 continuous panning servo make it stop
void Camera::Pan(int angle)
{
  //Stop limit 1465 to 1577
  //Full stop 1521
  //For mapping functionality use
  //_servoPan.writeMicroseconds((int)map(angle,0, 90,1409,1633));
  _servoPan.writeMicroseconds((int)map(angle,0, 200,1429,1613));
  //_servoPan.writeMicroseconds(angle);
  Serial.print("Camera Panned with speed: ");
  Serial.println(angle);
  panAngle = angle;
}

void Camera::Tilt(int angle)
{
  //For mapping functionality use
  //_servoTilt.writeMicroseconds((int)map(angle,0, 90,800,2200));
  _servoTilt.writeMicroseconds((int)map(angle,90, 0,900,2100));
  //_servoTilt.writeMicroseconds(angle);
  Serial.print("Camera Tilted with angle: ");
  Serial.println(angle);
  tiltAngle = angle;
}

int Camera::GetPan()
{
  return panAngle;
}

int Camera::GetTilt()
{
  return tiltAngle;
}
  
