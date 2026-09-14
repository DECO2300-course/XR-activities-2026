# Streaming Video from the Headset

> **Headset badge: headset required.** Everything here needs a Meta Quest 2 / 3 / 3S in
> developer mode, and a USB-C cable that carries data.

## Objective
Get a live view of what is happening inside the headset onto your computer screen.

You need this for the **Week 12 user testing** sessions, where we need to record what the
participant sees. You will also want it in **Week 9** testing too. 
Set it up and prove it works before either of those.

## Prerequisites
- A **Meta Quest 2 / 3S** with **developer mode** enabled.
- A **USB-C cable that carries data**. Charge-only cables fail in a way that looks like broken
  software: the headset charges and nothing else happens
- A build of your own project on the headset, so there is something to look at
- For Option 1 only: **Meta Quest Developer Hub**, signed in with an account.

## The two options

| | Needs | Wireless | Gives you |
|---|---|---|---|
| **1. Meta Quest Developer Hub** | Meta account, signed in | Yes, over ADB Wi-Fi | Cast, record and screenshot from the panel you already use to install builds |
| **2. Quest Live View** | Nothing but the download | No, USB only | A lens-corrected single-eye view, plus snapshots |

Option 1 is the one to reach for if you already have MQDH open. Option 2 is faster to start and needs no account.

---

## Option 1: Meta Quest Developer Hub

Everything below lives in **Device manager → Device Actions**, at the bottom of the window.

![Meta Quest Developer Hub, Device manager](2026-09-09%2011_44_07-Meta%20Quest%20Developer%20Hub.png)

### Cast over USB

1. Plug the headset in and put it on. Approve **Allow USB debugging** inside the headset,
   ticking **Always allow from this computer**.
2. The headset appears at the top of **Device manager** marked **Active**.
3. In **Device Actions**, press **Cast** on the *Cast Device* row.
4. A window opens showing the headset's view. **Stop** ends it.

Two neighbouring rows do the related jobs:

- **Record Video → Record** writes a file. The cog beside it sets resolution and bitrate.
- **Screenshot → Capture** takes a still.

Both save to the folder set in **Settings**, and appear under **File manager**.

### Enable ADB over Wi-Fi

This is what makes everything else wireless. The headset has to be on USB to start with.

1. With the headset connected by cable, find **ADB over Wi-Fi** in the right-hand column of
   **Device Actions**.
2. Check the network dropdown next to the headset's name — in the screenshot it reads
   `Deco2300`. **Your computer must be on that same network.**
3. Toggle **ADB over Wi-Fi** to **On**.
4. Unplug the cable. The headset stays listed as **Active**.

`adb devices` now shows an entry like `192.168.1.34:5555` instead of a serial number. Toggling
the switch off returns you to USB.


### Cast over Wi-Fi

With ADB over Wi-Fi on and the cable removed, press **Cast** exactly as before. Record and
Screenshot also keep working.

Wireless costs you latency and bitrate. Use it when the wearer needs to walk around, not when
you are judging whether your frame rate is smooth.

---

## Option 2: Quest Live View

A small application written for this module. Download it from **Blackboard → Tools**. It needs
no account and no sign-in.

![Quest Live View, streaming a scene from a Quest 3S](2026-09-09%2011_26_19-Quest%20Live%20View%201.0.0.png)

1. Download the file for your machine: the `-windows-x64-setup.exe` for Windows, or the
   `macos-arm64` disk image for an Apple silicon Mac. Both are self-contained — nothing else to
   install, no Python, no adb, no scrcpy.
2. Unpack it and run `QuestLiveView`. Windows SmartScreen warns that the publisher is unknown,
   because the build is unsigned: **More info → Run anyway**. On macOS the first launch is
   refused for the same reason; **System Settings → Privacy & Security → Open Anyway**.
3. Plug the headset in, put it on, and approve **Allow USB debugging** inside the headset,
   ticking **Always allow from this computer**.
4. Press **Start**.

The controls along the bottom:

| Control | Does |
|---|---|
| **View: Corrected / Raw** | Corrected undoes the lens distortion. Raw shows the panel as it is |
| **Eye** | Which eye to show |
| **Correction** | Trims the amount of undistortion if the edges look wrong |
| **Quality / fps** | Lower both if the stream stutters. Resolution is the lever that matters |
| **Snapshot** | Saves the current frame |

The status line reports what it measured — `crop 5196:7846:6:8` in the screenshot. It finds the
image bounds on your headset at run time rather than assuming them, so it works on Quest models
it has never seen.

**USB only.** It has been built and tested over a cable. It talks to the headset through `adb`,
so a wireless `adb` connection may work, but nobody has confirmed it — do not plan around it.

---

## A warning about networks

**The university network will probably not carry a wireless stream.** It is inconsistent, and
campus networks routinely stop devices talking to each other directly, which blocks ADB over
Wi-Fi outright.

The workaround that tends to work is a **phone hotspot**: put the headset and your laptop both
on your own hotspot, then enable ADB over Wi-Fi. It is a small, private network with nothing in
the way.

**Test this before the session you need it for.** Find out in Week 9 whether wireless works
where you will be sitting in Week 12, not on the morning itself. A USB cable is the fallback
that always works, so bring one.

---

## Other routes

Worth knowing about, but reach for them second.

### scrcpy

[scrcpy](https://github.com/Genymobile/scrcpy) is the open-source Android mirror that Quest Live
View is built on top of. Running it directly gives you the raw panel and more flags.

```
scrcpy -m 1280
```

**Always pass a size.** The Quest reports an override display size of 10992 x 8000, which no
hardware encoder accepts. Plain `scrcpy` prints two `Capture/encoding error` lines and recovers
on its own; `-m 1280` skips the detour.

| Flag | Effect |
|---|---|
| `-m 1280` | Caps the long edge |
| `--record=demo.mp4` | Writes the stream to a file while you watch |
| `--time-limit=30` | Stops after a set number of seconds |
| `--crop=5496:8000:0:0` | A `width:height:x:y` region in device pixels — here, the left eye |
| `--tcpip` | Switches the headset to wireless and connects, in one step |

### Record on the headset

For a clip made on the device rather than streamed off it:

```
adb shell screenrecord --size 1280x720 /sdcard/demo.mp4
adb pull /sdcard/demo.mp4
```

`Ctrl+C` stops the recording. Without `--size` it tries the 10992 x 8000 override, prints
`unable to configure video/avc codec`, and falls back to 1280 x 720 anyway.



## References
- Meta Quest Developer Hub: <https://developers.meta.com/horizon/documentation/unity/ts-odh/>
- MQDH device manager, including ADB over Wi-Fi and casting: <https://developers.meta.com/horizon/documentation/unity/ts-odh-device-manager/>
- scrcpy: <https://github.com/Genymobile/scrcpy>
- scrcpy video options, including `--crop` and `--max-size`: <https://github.com/Genymobile/scrcpy/blob/master/doc/video.md>
- scrcpy wireless connection: <https://github.com/Genymobile/scrcpy/blob/master/doc/connection.md>
- Android platform-tools (`adb`): <https://developer.android.com/tools/releases/platform-tools>
- Render Texture: <https://docs.unity3d.com/6000.3/Documentation/Manual/class-RenderTexture.html>
