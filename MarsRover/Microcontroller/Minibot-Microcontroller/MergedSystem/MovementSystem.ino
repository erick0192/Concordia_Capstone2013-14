void Init_Wheels()
{
    pinMode(TOP_RIGHT_WHEEL_ENABLE_PIN, OUTPUT);      
    pinMode(TOP_RIGHT_WHEEL_FRONT_PIN, OUTPUT);    
    pinMode(TOP_RIGHT_WHEEL_BACK_PIN, OUTPUT);    
      
    pinMode(TOP_LEFT_WHEEL_ENABLE_PIN, OUTPUT);      
    pinMode(TOP_LEFT_WHEEL_FRONT_PIN, OUTPUT);    
    pinMode(TOP_LEFT_WHEEL_BACK_PIN, OUTPUT);  
      
    pinMode(BOTTOM_RIGHT_WHEEL_ENABLE_PIN, OUTPUT);      
    pinMode(BOTTOM_RIGHT_WHEEL_FRONT_PIN, OUTPUT);    
    pinMode(TOP_LEFT_WHEEL_BACK_PIN, OUTPUT);    
      
    pinMode(BOTTOM_LEFT_WHEEL_ENABLE_PIN, OUTPUT);      
    pinMode(BOTTOM_LEFT_WHEEL_FRONT_PIN, OUTPUT);    
    pinMode(TOP_LEFT_WHEEL_BACK_PIN, OUTPUT);
}

void MoveForwardLeft(int mSpeed)
{
    analogWrite(TOP_RIGHT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(TOP_RIGHT_WHEEL_FRONT_PIN, HIGH);
    digitalWrite(TOP_RIGHT_WHEEL_BACK_PIN, LOW);
    
    analogWrite(TOP_LEFT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(TOP_LEFT_WHEEL_FRONT_PIN, HIGH);
    digitalWrite(TOP_LEFT_WHEEL_BACK_PIN, LOW);
}

void MoveForwardRight(int mSpeed)
{
    analogWrite(BOTTOM_RIGHT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(BOTTOM_RIGHT_WHEEL_FRONT_PIN, LOW);
    digitalWrite(BOTTOM_RIGHT_WHEEL_BACK_PIN, HIGH);
    
    analogWrite(BOTTOM_LEFT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(BOTTOM_LEFT_WHEEL_FRONT_PIN, LOW);
    digitalWrite(BOTTOM_LEFT_WHEEL_BACK_PIN, HIGH);
}

void MoveBackwardLeft(int mSpeed)
{
    analogWrite(TOP_RIGHT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(TOP_RIGHT_WHEEL_FRONT_PIN, LOW);
    digitalWrite(TOP_RIGHT_WHEEL_BACK_PIN, HIGH);
    
    analogWrite(TOP_LEFT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(TOP_LEFT_WHEEL_FRONT_PIN, LOW);
    digitalWrite(TOP_LEFT_WHEEL_BACK_PIN, HIGH);
}

void MoveBackwardRight(int mSpeed)
{
  
    analogWrite(BOTTOM_RIGHT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(BOTTOM_RIGHT_WHEEL_FRONT_PIN, HIGH);
    digitalWrite(BOTTOM_RIGHT_WHEEL_BACK_PIN, LOW);
    
    analogWrite(BOTTOM_LEFT_WHEEL_ENABLE_PIN, mSpeed);
    digitalWrite(BOTTOM_LEFT_WHEEL_FRONT_PIN, HIGH);
    digitalWrite(BOTTOM_LEFT_WHEEL_BACK_PIN, LOW);
}

void StopMove()
{
  MoveForwardLeft(0);
  MoveForwardRight(0);
}
