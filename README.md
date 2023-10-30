![](https://github.com/sudipmandal/YOLOv8PersonDetectionAPI/actions/workflows/docker-publish.yml/badge.svg)
![](https://github.com/sudipmandal/YOLOv8PersonDetectionAPI/actions/workflows/codeql.yml/badge.svg)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
# Person Detection API using YOLOv8
A simple REST API to detect if any person is present in an image using the [YOLOv8](https://github.com/ultralytics/ultralytics) library

## Abstract
- I have a Reolink doorbell camera which is capable of uploading images to a ftp server at periodic intervals
- Even though Reolink mobile app has ability to notify on human/motion detection, it does so only on its own app, there is no easy way to hook to this or extend this capability for home automation scenarios like using Home Assistant for example, I needed the notification to be sent to my PC and also trigger custom automations.
- The solution needed to be dead simple and capable of analyzing the images locally and not relying on any cloud service
- The solution needed to work on low end home servers running without any gpus (ideally as a docker container)
- At the time of this writing, I could not find a readily available option to meet my needs perfectly and hence I decided to write this simple api myself.
- This api accepts the image as a base64 string and processes it using the YOLOv8 library and returns the count of persons if any in the image
- API is fairly fast and using only 2 cores of my Intel n95 cpu and running as docker container it is able to analyze most images (size < 5 MB) in under 2 seconds.
- I use this api along with [n8n](https://github.com/n8n-io/n8n) and [ntfy](https://github.com/binwiederhier/ntfy) to receive near real time notifications on my PC (and a bunch of other home assistant automations)

## Installation/Self Hosting
Latest Docker Container Image available on Docker Hub [sudipthegreat/yolov8_person_detection_api](https://hub.docker.com/r/sudipthegreat/yolov8_person_detection_api)

```
docker pull sudipthegreat/yolov8_person_detection_api
docker run -p 80:80 --name persondetectapi --restart=always --shm-size=1024m  sudipthegreat/yolov8_person_detection_api
```
## Sample HTTP Request
```
POST http://{{SERVER-HOST}}/api/person-detection

Accept : *
Content-Type : application/json
{
   "Base64Image": {{ BASE64 IMG STRING }}
}
```
## Sample Response
```
{
    "numberOfHumans": 2
}
```

## Credits
This api would not be possible without these awesome open source projects
- [Ultralytics](https://github.com/ultralytics/ultralytics) 
- [YOLOv8](https://github.com/dme-compunet/YOLOv8) 
- [ONXX Runtime](https://github.com/microsoft/onnxruntime)
- [Dotnet Core](https://github.com/dotnet/core)

## Roadmap
None at the moment. <br/> This is a hobby project developed to meet a home automation need and is not intended for production use. <br/>
Please feel free to send PR if you feel like improving on this work.


