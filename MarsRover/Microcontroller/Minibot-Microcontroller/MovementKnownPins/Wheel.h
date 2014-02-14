#ifndef Wheel_h
#define Wheel_h

class Wheel {
	private:
	  int _enablePin;
	  int _frontPin;
	  int _backPin;

	public:
	  Wheel(int enablePin, int frontPin, int backPin);
	  Wheel();
	  int getEnablePin();
	  int getFrontPin();
	  int getBackPin();
	  void setEnablePin(int enablePin);
	  void setFrontPin(int frontPin);
	  void setBackPin(int backPin);
      void setAll(int data[]); //Expected data order: [enable pin, front pin, back pin]
};
#endif
