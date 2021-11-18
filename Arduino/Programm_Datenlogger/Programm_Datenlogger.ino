#include <Adafruit_BME280.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <math.h>
#include <EEPROM.h>
#include <MCP342x.h>

#define F_CPU 16000000

#define ALARM 10
#define ADC0 A0
#define ADC1 A1
#define ADC2 A2
#define ADC3 A3
#define ADC4 A8
#define ADC5 A9
#define ADC6 A10
#define ADC7 A11
#define ADC8 A12
#define ADC9 A13
#define ADC10 A14
#define ADC11 A15

#define DIO0 49
#define DIO1 48
#define DIO2 47
#define DIO3 46
#define DIO4 45
#define DIO5 44
#define DIO6 43
#define DIO7 42
#define DIO8 37
#define DIO9 36
#define DIO10 35
#define DIO11 34
#define DIO12 33
#define DIO13 32
#define DIO14 31
#define DIO15 35

#define DIO16 22
#define DIO17 23
#define DIO18 24
#define DIO19 25
#define DIO20 26
#define DIO21 27
#define DIO22 28
#define DIO23 29

//Settings festlegen
int measurements = 1;
double lsbinadc = 0.0047939453;
double lsbexadc = 0.000015625;
long baudrate = 115200;

long val1;
long val2;
long val3;
long val4;
long rnd_nr;

double valadc1;
double valadc2;
double valadc3;
double valadc4;
double valadc5;
double valadc6;
double valadc7;
double valadc8;
double erg;
double average;



Adafruit_BME280 bme;

StaticJsonDocument<2000> doc;

//ADC einstellen
// 0x68 is the default address for all MCP342x devices
uint8_t address = 0x68;
MCP342x adc = MCP342x(address);
MCP342x::Config status;
bool startConversion = false;


void setup()
{
  CLKPR = 0x80;
  //Referenzspannung einstellen
  analogRead(EXTERNAL);

  //Zähler einstellen
  rnd_nr = EEPROM.read(1);
  rnd_nr = rnd_nr + 1;
  EEPROM.write(1, rnd_nr);
  if (rnd_nr > 250)
  {
    rnd_nr = 0;
    EEPROM.write(1, rnd_nr);
  }

  Serial.begin(baudrate);
  Serial1.begin(baudrate);
  Serial2.begin(baudrate);
  Serial3.begin(baudrate);
  pinMode(ALARM, OUTPUT);
  pinMode(22, OUTPUT);
  pinMode(23, OUTPUT);
  pinMode(24, OUTPUT);
  pinMode(25, OUTPUT);
  pinMode(26, OUTPUT);
  pinMode(27, OUTPUT);
  pinMode(28, OUTPUT);
  pinMode(29, OUTPUT);

  //BME Initialisierung
  if (!bme.begin(0x76)) {
    Serial.println("Could not find a valid BME280 sensor, check wiring!");
    while (1);
  }

  //ADC Initialisierung
  MCP342x::generalCallReset();
  delay(1); // MC342x needs 300us to settle
  Wire.requestFrom(address, (uint8_t)1);
  if (!Wire.available()) {
    Serial.print("No device found at address ");
    Serial.println(address, HEX);
    while (1);
    startConversion = true;
  }
}



void loop() {

  //Json initialisieren
  JsonObject Measure = doc.to<JsonObject>();
  Measure["SessionId"] = rnd_nr;
  JsonArray Measurements  = Measure.createNestedArray("Measurements");
  //Variablen nullen
  valadc1 = 0;
  valadc2 = 0;
  valadc3 = 0;
  valadc4 = 0;
  valadc5 = 0;
  valadc6 = 0;
  valadc7 = 0;
  valadc8 = 0;
  //Messwerte einsammeln und addieren
  for (int i = 0; i < measurements; i++)
  {
    adc.convertAndRead(MCP342x::channel1, MCP342x::oneShot, MCP342x::resolution18, MCP342x::gain1, 1000000, val1, status);
    valadc1 = valadc1 + val1;
    adc.convertAndRead(MCP342x::channel2, MCP342x::oneShot, MCP342x::resolution18, MCP342x::gain1, 1000000, val2, status);
    valadc2 = valadc2 + val2;
    adc.convertAndRead(MCP342x::channel3, MCP342x::oneShot, MCP342x::resolution18, MCP342x::gain1, 1000000, val3, status);
    valadc3 = valadc3 + val3;
    adc.convertAndRead(MCP342x::channel4, MCP342x::oneShot, MCP342x::resolution18, MCP342x::gain1, 1000000, val4, status);
    valadc4 = valadc4 + val4;
    valadc5 = valadc5 + analogRead(8);
    valadc6 = valadc6 + analogRead(9);
    valadc7 = valadc7 + analogRead(10);
    valadc8 = valadc8 + analogRead(11);
  }
  //Addierte Messwerte auswerten
  JsonObject measure1  = Measurements.createNestedObject();
  measure1["SensorId"] = "1";
  measure1["Value"] = CalcAverage(valadc1, lsbexadc);
  JsonObject measure2  = Measurements.createNestedObject();
  measure2["SensorId"] = "2";
  measure2["Value"] =  CalcAverage(valadc2, lsbexadc);
  JsonObject measure3  = Measurements.createNestedObject();
  measure3["SensorId"] = "3";
  measure3["Value"] =  CalcAverage(valadc3, lsbexadc);
  JsonObject measure4  = Measurements.createNestedObject();
  measure4["SensorId"] = "4";
  measure4["Value"] =  CalcAverage(valadc4, lsbexadc);
  JsonObject measure5  = Measurements.createNestedObject();
  measure5["SensorId"] = "5";
  measure5["Value"] =  CalcAverage(valadc5, lsbinadc);
  JsonObject measure6  = Measurements.createNestedObject();
  measure6["SensorId"] = "6";
  measure6["Value"] =  CalcAverage(valadc6, lsbinadc);
  JsonObject measure7  = Measurements.createNestedObject();
  measure7["SensorId"] = "7";
  measure7["Value"] =  CalcAverage(valadc7, lsbinadc);
  JsonObject measure8  = Measurements.createNestedObject();
  measure8["SensorId"] = "8";
  measure8["Value"] =  CalcAverage(valadc8, lsbinadc);

  //Temperatur übergeben in °C
  JsonObject measure9  = Measurements.createNestedObject();
  measure9["SensorId"] = "9";
  measure9["Value"] =  bme.readTemperature();
  //Druck übergeben in hPa
  JsonObject measure10  = Measurements.createNestedObject();
  measure10["SensorId"] = "10";
  measure10["Value"] =  bme.readPressure() / 100;
  //Luftfeuchtigkeit übergeben in Prozent
  JsonObject measure11  = Measurements.createNestedObject();
  measure11["SensorId"] = "11";
  measure11["Value"] =  bme.readHumidity();
  serializeJson(doc, Serial);
  serializeJson(doc, Serial1);
  serializeJson(doc, Serial2);
  serializeJson(doc, Serial3);
  Serial.println("");
  Serial1.println("");
  Serial2.println("");
  Serial3.println("");
}
//Mittelwert berechnen
double CalcAverage(int adc, double lsb)
{
  erg = 0;
  erg = adc / measurements;
  average = erg * lsb;
  return average;
}
