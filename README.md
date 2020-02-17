# ShhhSMS
Xamarin.Android Application facilitating the sending and receiving of encrypted SMS messages

## Overview
There is a lot of pressure on 'the big technology companies' like WhatsApp to put backdoors into their applications or somehow weaken the levels of encryption but I feel that this is misguided.

Fortunately the technology companies are pushing back but how long will they be able to keep this up?

The problem is that if say WhatsApp were to reduce the levels of privacy provided by their platform then the bad guys would just move onto something else - leaving the rest of us on a hobbled platform, at the mercy of hackers etc.

I've previously written a command line utility that will encrypt text and files using the libSodium encryption library. But while us techies may be comfortable on the Command Line, the general public are not - and privacy should be for everyone.

So, in an effort to demonstrate the futility of hobbling platforms such as WhatsApp, to show that the encryption genie is very much out of the bottle and cannot be put back in.

## How it works
ShhhSMS works by encrypting messages using private/public keys before handing the resulting text to the devices built-in SMS application.

When a ShhhSMS message is received by the recipient they can use the SMS applications built-in functionality to share the encrypted text with ShhhSMS - which will then attempt to decrypt it and display it on screen for the user.

The key difference here is that the the app uses the mobile carriers SMS channel to send the messages and they are not responsible for any part of the encryption - so they cannot be pressured to provide access to the decrpyted text.
