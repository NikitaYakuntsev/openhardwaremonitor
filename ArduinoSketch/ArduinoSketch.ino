#include <ArduinoJson.h>

enum HwType { CPU = 2, RAM = 3, GPU = 4 };
HwType currentHwType = CPU;
int currentShowState = 1; //increment this later on btn click.

int ind100Pin = 5;
int ind300Pin = 6;

int ind300r = 11;
int ind300g = 10;

const int PERCENT_LIMIT = 100;
const int TEMP_LIMIT = 100;
const int PIN_LIMIT = 255;

void setup() {
  Serial.begin(9600);
  while (!Serial) {
    // wait serial port initialization
  }  
  pinMode(ind100Pin, OUTPUT);
  pinMode(ind300Pin, OUTPUT);
  
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
    int showState = curr["GroupId"];
    double value;
    int type;
    int indicator;
    if (showState == currentShowState) {
      value = curr["Value"];
      type = curr["Type"];
      indicator = curr["PrefIndicator"];
      
      bool ind300 = indicator == 300;
      int writePin = ind300 ? ind300Pin : ind100Pin;
      int writeValue = map(value, 0, ind300 ? TEMP_LIMIT : PERCENT_LIMIT, 0, PIN_LIMIT);
      analogWrite(writePin, writeValue);
      int r;
      int g;
      int b;
      HSV_to_RGB(map(value, 0, 100, 85, 0), 200, &r, &g, &b);
      if (ind300) {  
        analogWrite(ind300r, r);
        analogWrite(ind300g, g);
      }
    }       
  }
  
   

}

void Init() {
  digitalWrite(ind100Pin, HIGH);
  digitalWrite(ind300Pin, HIGH);  
  delay(500);
  digitalWrite(ind100Pin, LOW);
  digitalWrite(ind300Pin, LOW);
  delay(500);
  digitalWrite(ind100Pin, HIGH);
  digitalWrite(ind300Pin, HIGH);  
  delay(500);
  digitalWrite(ind100Pin, LOW);
  digitalWrite(ind300Pin, LOW);
  delay(500);
}


void HSV_to_RGB(double hue, double brightness, int *r, int *g, int *b)
{
   int R, G, B;
   unsigned int scaledHue = (hue * 3);
   unsigned int segment = scaledHue / 256; // segment 0 to 5 around the color wheel
   unsigned int segmentOffset = scaledHue - (segment * 256);      // position within the segment
   
   unsigned int compliment = 0;
   unsigned int prev = (brightness * ( 255 -  segmentOffset)) / 256;
   unsigned int next = (brightness *  segmentOffset) / 256;


   
   switch(segment ) {
   case 0:      // red
      R = brightness;
      G = next;
      B = compliment;
     break;
   case 1:     // yellow
     R = prev;
     G = brightness;
     B = compliment;
     break;
   case 2:     // green
     R = compliment;
     G = brightness;
     B = next;
     break;
   }
   R = min(255, (int)(R*1.1));
//    Serial.print("V: "); Serial.println(hue);
//   Serial.print("R: "); Serial.println(R);
//   Serial.print("G: "); Serial.println(G);
//   Serial.print("B: "); Serial.println(B);
   *r = R;
   *g = G;
   *b = B;
}
