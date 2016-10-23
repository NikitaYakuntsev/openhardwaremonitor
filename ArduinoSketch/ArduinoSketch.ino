#include <ArduinoJson.h>

enum HwType { CPU = 2, RAM = 3, GPU = 4 };
HwType currentHwType = GPU;

int percentPin = 5;
int tempPin = 6;
const int PERCENT_LIMIT = 100;
const int TEMP_LIMIT = 100;
const int PIN_LIMIT = 255;

void setup() {
  Serial.begin(9600);
  while (!Serial) {
    // wait serial port initialization
  }  
  pinMode(percentPin, OUTPUT);
  pinMode(tempPin, OUTPUT);
  
  Init();
}

void loop() {
  // not used in this example
}

void serialEvent()
{
  char* json;
  String str = Serial.readStringUntil('\n');
  json = (char*) str.c_str();
  DynamicJsonBuffer jsonBuffer;
  Serial.println(json);
  JsonArray& root = jsonBuffer.parse(json);

  // Test if parsing succeeds.
  if (!root.success()) {
    return;
  }
  
  for (int i = 0; i < root.size(); i++) {
    JsonObject& curr = root[i];
    int hwType = curr["HwType"];
    double value;
    int type;
    if (hwType == currentHwType) {
      value = curr["Value"];
      type = curr["Type"];
      
      bool temp = type == 2;
      int writePin = temp ? tempPin : percentPin;
      int writeValue = map(value, 0, temp ? TEMP_LIMIT : PERCENT_LIMIT, 0, PIN_LIMIT);
      analogWrite(writePin, writeValue);
    }       
  }
   

}

void Init() {
  digitalWrite(percentPin, HIGH);
  digitalWrite(tempPin, HIGH);  
  delay(500);
  digitalWrite(percentPin, LOW);
  digitalWrite(tempPin, LOW);
  delay(500);
  digitalWrite(percentPin, HIGH);
  digitalWrite(tempPin, HIGH);  
  delay(500);
  digitalWrite(percentPin, LOW);
  digitalWrite(tempPin, LOW);
  delay(500);
}


