char incomingByte = 0;   // for incoming serial data
String commandString = "";
boolean commandStarted = false;
int angle = 0;
char idCamera = 0;
void setup()
{
  Serial.begin(9600);
}

void loop()
{
          // send data only when you receive data:
        if (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read();
                if(incomingByte == '<' || commandStarted == true)
                {
                  commandStarted = true;
                  commandString = commandString + incomingByte;
                }
                if(incomingByte == '>')
                {
                  if(commandString[1] == 'P')
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();

                      Serial.print("Pan camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle);
                    }
                  
                  if(commandString[1] == 'T')
                    {
                      idCamera = commandString[2];
                      angle = commandString.substring(3, commandString.indexOf('>')).toInt();

                      Serial.print("Tilt camera ");
                      Serial.print(idCamera);
                      Serial.print(" with angle:");
                      Serial.println(angle);
                    }
                    
                  commandStarted = false;
                  commandString = "";
                }
        }
  /*
    if(Serial.available())
  {
  char *p = sz;
  char *str;
  Serial.begin(9600);
  while ((str = strtok_r(p, ";", &p)) != NULL) // delimiter is the semicolon
    Serial.println(str);
  }
  */
}
