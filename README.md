# IdentifyMe

A type Aries Mobile Agent.
  - Self-Sovereign Identity.
  - Integrate running a deployed on heroku mediator.
  - Can Get Issue message, Present Proof, Notification.

## APK Realse
 Download APK Relase here:
 https://drive.google.com/drive/folders/1Df4nsNXw9XvBGhCM6SKEwKqoNMemcHqQ?usp=sharing
 
## Run as a Project
 Download libindy for android and copy to folder IdentifyMe.Android/libs:
 https://drive.google.com/drive/folders/1Bq-Oq058FqvNoPW9QBtQ0-_NMz0e3MDn
## Bug
 + Due to the limitation of heroku, mediator will be sleep after 30 minutes don't have any request and after that, heroku will clear all database. Connection between app and mediator will be lost sothat you can't not get any message. Clear app data and reinstall will work fine :(
 + If you Issue a decimal value, and want to validate with predicates value, this will through some error.

 with a [public repository][dill]
 on GitHub.

# Many Thanks 
 This project is developed base on the Open Source Mobile agent: https://github.com/mattrglobal/osma
 And many thanks to the Hyperledger Aries Community that help me a lot so that I could done this project.


