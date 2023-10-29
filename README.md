# Person Detection API using YOLOv8
A simple API to detect if any person is present in an image using the [YOLOv8](https://github.com/ultralytics/ultralytics) library

## Abstract
- I have a Reolink doorbell camera which is capable of uploading images to a ftp server at periodic intervals
- Even though Reolink mobile app has ability to notify on human detection/motion detection, I needed the notification to be sent to my PC and also be able to do custom stuff in home assistant
- I needed a docker based simple api which could quickly analyze the uploaded images and detect any human presence (goal is to get notified if present)
- My home lab server is a low powered cpu only server hence the api needed to perform well using only cpu and needed to be fairly quick
- The above api accepts the image as a base64 string and processes it using the YOLOv8 library and returns the count of persons if any in the image
- API is fairly fast and using only 2 cores of my Intel n95 cpu is able to analyze most images (size < 5 MB) in under 2 seconds.
- I use this api along with [n8n](https://github.com/n8n-io/n8n) and [ntfy](https://github.com/binwiederhier/ntfy) to receive near real time notifications on my PC (and a bunch of other home assistant automations)

## Installation/Self Hosting
Latest Docker Container Image available on Docker Hub [sudipthegreat/yolov8_person_detection_api](https://hub.docker.com/r/sudipthegreat/yolov8_person_detection_api)
 
`docker pull sudipthegreat/yolov8_person_detection_api` <br/>
`docker run -p 80:80 --name persondetectapi --restart=always --shm-size=1024m  sudipthegreat/yolov8_person_detection_api`

## Sample HTTP Request

`POST http://{{SERVER-HOST}}/api/person-detection`<br /><br />
`Accept : *` <br />
`Content-Type : application/json` <br /><br/>
`{` <br />
`   "Base64Image": {{ BASE64 IMG STRING }}` <br />
`}`

## Sample Response
`{` <br />
`    "numberOfHumans": 2` <br/>
`}`

## Credits
This api would not be possible without these awesome open source projects
- [Ultralytics](https://github.com/ultralytics/ultralytics) 
- [YOLOv8](https://github.com/dme-compunet/YOLOv8) 
- [ONXX Runtime](https://github.com/microsoft/onnxruntime)
- [Dotnet Core](https://github.com/dotnet/core)

## Roadmap
None at the moment. <br/> This is a hobby project developed to meet a home automation need and is not intended for production use. <br/>
Please feel free to send PR if you feel like improving on this work.


