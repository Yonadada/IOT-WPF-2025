## 파이썬 MQTT Publish
# paho-mqtt 라이브러리 설치
# pip install paho-mqtt

import paho.mqtt.client as mqtt
import datetime as dt
import uuid
import random
import time

import json
from collections import OrderedDict



PUB_ID = 'IOT61' # 본인 아이피 마지막 주소
BROKER = '210.119.12.61' # 본인 아이피
PORT = 1883
TOPIC = 'smarthome/61/topic' # publish/subscribe에서 사용할 토픽 => 핵심
COLORS = ['RED', 'ORANGE', 'YELLOW', 'GREEN', 'BLUE', 'NAVY', 'PURPLE']
COUNT = 0

# [Fake] 센서 설정
SENSOR1 = "온도 센서 세팅"; PIN = 42
SENSOR2 = "포토 센서 세팅"; PIN2 = 7
SENSOR3 = "워터드롭 센서 세팅"; PIN3 = 9
SENSOR4 = "인체감지 센서 세팅"; PIN4 = 11    


# 연결 콜백
def on_connect(client, userdata, flags, reson_code, properties=None):
    print(f'Connected with reason code : {reson_code}')

# 퍼블리시 완료 후 발생 콜백
def on_publish(client, userdata, mid):
    print(f'Message published mid : {mid}')

try:
    client = mqtt.Client(client_id=PUB_ID, protocol=mqtt.MQTTv5)
    client.on_connect = on_connect
    client.on_publish = on_publish

    client.connect(BROKER, PORT)
    client.loop_start() # 평생 돌아

    while True:
        # 퍼블리시
        currtime = dt.datetime.now()
        selected = random.choice(COLORS)
        temp = random.uniform(20.0, 29.9) # [Fake] 실제로는 센서에서 값을 받아옴
        humid = random.uniform(40.0, 65.9) # [Fake] 실제로는 센서에서 값을 받아옴
        rain = random.randint(0, 1) # [Fake] 실제로는 센서에서 값을 받아옴. 0은 맑음 1은 비
        detect = random.randint(0, 1) #[Fake] 인체센서, 실제로는 센서에서 값을 받아옴
        photo = random.randint(50, 255) #[Fake] 포토센서, 255 쪽이 어두움 센싱
        
        COUNT += 1
        ## 센싱 데이터를 json 형태로 변경
        ## OrderedDict로 먼저 구성 
        raw_data = OrderedDict() # 순서가 있는 딕셔너리타입 객체 
        raw_data['PUP_ID']  =  PUB_ID
        raw_data['COUNT'] = COUNT
        raw_data['SENSING_DT'] = currtime.strftime(f'%Y-%m-%d, %H:%M:%S') # c#의 'yyyy-MM-dd HH:MM:SS' 같음
        raw_data['TEMP'] = f'{temp:0.1f}' # 소수점 1자리만 나오도록 
        raw_data['HUMID'] = f'{humid:0.1f}'
        raw_data['LIGHT'] = 1 if photo >= 200 else 0
        raw_data['HUMAN'] = detect
        #python 딕셔너리 형태로 저장되어 있음. json이랑 거의 똑같음 
        
        ## OrderedDict -> json 타입으로 변경
        pub_data = json.dumps(raw_data, ensure_ascii=False, indent='\t')
        
        ## payload에 json 데이터를 할당
        client.publish(TOPIC, payload=pub_data, qos=1)
        time.sleep(1)

except Exception as ex:
    print(f'Error raised : {ex}')
except KeyboardInterrupt:
    print('MQTT 전송 중단')
    client.loop_stop()
    client.disconnect()