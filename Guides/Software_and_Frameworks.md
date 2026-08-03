# Software and Frameworks

**Semester 2, 2026**

Everything you need to install, in one place, with the official documentation for each.
Install in the order below. If something goes wrong during setup, the two step-by-step
guides in this folder cover the process in detail — this page is the reference list, not
the walkthrough.

---

## The stack at a glance

| Layer | What we use | Platform |
|---|---|---|
| Version control | GitHub account + GitHub Desktop | Windows, macOS |
| Engine | Unity 6.3 LTS + Unity Hub | Windows, macOS |
| Build target | Android Build Support (Quest runs Android) | Windows, macOS |
| XR runtime | Unity OpenXR Plugin | Windows, macOS |
| XR interaction | Unity XR Interaction Toolkit | Windows, macOS |
| In-Editor testing | XR Device Simulator (ships with the Interaction Toolkit) | Windows, macOS |
| Device tooling | Meta Quest Developer Hub (MQDH) | Windows, macOS |
| Headset link (optional, not taught) | Meta Quest Link | **Windows only** |
| Headsets | Meta Quest 2, Quest 3, Quest 3S | — |

> **This course is OpenXR, all the way down.** We use the open, vendor-neutral standard
> and Unity's own XR Interaction Toolkit — **not** the Meta XR All-in-One SDK and **not**
> Building Blocks. Most Quest tutorials you find online use Meta's SDK; they are not
> wrong, they are just a different stack, and mixing the two will cause you problems.
> Follow the course guides. The reasoning is explained in the OpenXR setup guide.

> **Mac users, read this first.** Everything in this course works on a Mac, but you
> cannot use Quest Link to play the headset directly from the Editor — that feature is
> Windows-only. Instead you have two paths: the **XR Device Simulator** for fast
> iteration in the Editor, and building an `.apk` to the headset for the real thing.
> Neither is optional — the simulator is fast but only the headset tells you the truth.
> The OpenXR setup guide covers every path.

---

## 1. Version control

### GitHub account
- Sign up: <https://github.com>
- Use your **student email** so you can claim the education benefits below.
- Student benefits (free private repos, Copilot, etc.): <https://education.github.com/pack>

### GitHub Desktop
The course uses the desktop app, not the command line.

- Download: <https://desktop.github.com>
- Documentation: <https://docs.github.com/en/desktop>

### Git LFS (only if you need it)
For the rare large binary asset that genuinely must live in the repo.

- <https://git-lfs.com>
- Prefer keeping large files out of Git entirely — see the Unity + GitHub guide.

**Setup walkthrough:** `Unity_GitHub_Course_Guide_V1.pdf`

---

## 2. Unity

### Unity Hub
Manages Editor versions and licences. Always launch projects through the Hub.

- Download: <https://unity.com/download>

### Unity Editor 6.3 LTS
Install through Unity Hub → **Installs** → **Install Editor** → choose the **6.3 LTS**
release (Unity Hub also shows this as a `6000.3.x` version number).

Everyone on the course uses the same major version. Mixing versions across machines will
force a project upgrade and can break the project for whoever opens it next.

**Modules to tick during installation:**

- **Android Build Support**
  - **OpenJDK**
  - **Android SDK & NDK Tools**
- **Windows Build Support (IL2CPP)** — Windows machines only, for PC-linked play mode
- **Documentation** — optional, gives you offline manual access

If you already installed Unity without these, you can add them later: Unity Hub →
**Installs** → the ⚙ / three-dot menu on your version → **Add modules**.

### Licence
- On lab PCs the licence is already applied — you should not need to sign in.
- On a personal machine, sign in to Unity Hub and use a **Unity Personal** or **Unity
  Student** licence.
- Student licence: <https://unity.com/products/unity-student>

### Documentation
- Unity Manual: <https://docs.unity3d.com/Manual/index.html>
- Scripting API: <https://docs.unity3d.com/ScriptReference/index.html>
- Unity Learn (free structured tutorials): <https://learn.unity.com>
- Unity Discussions (Q&A / forums): <https://discussions.unity.com>

**Setup walkthrough:** `Unity_GitHub_Course_Guide_V1.pdf`

---

## 3. Code editor

Unity needs an external editor for C#. Pick one — you do not need both.

### Visual Studio Code (recommended, cross-platform)
- Download: <https://code.visualstudio.com>
- Install the **C# Dev Kit** and **Unity** extensions from the marketplace.
- Set it in Unity: **Edit → Preferences → External Tools** (Windows) or
  **Unity → Settings → External Tools** (macOS).

### JetBrains Rider
Free with a student licence, and the strongest Unity tooling available.

- Download: <https://www.jetbrains.com/rider/>
- Student licence: <https://www.jetbrains.com/community/education/>

---

## 4. XR packages (installed inside Unity, not downloaded separately)

These are added per project through Unity's **Package Manager**, not installed
system-wide. You do not need them until you start XR work.

Everything below is a **Unity package on the OpenXR standard**. There is no Meta SDK in
this list, and that is deliberate — see the note at the end of this section.

### XR Plugin Management (`com.unity.xr.management`)
The Project Settings panel where you enable OpenXR per build target. Install this first.

- Docs: <https://docs.unity3d.com/Packages/com.unity.xr.management@latest>

### Unity OpenXR Plugin (`com.unity.xr.openxr`)
The cross-vendor XR runtime. This is what makes the project talk to the headset, and it
supplies the **Meta Quest Support** feature you enable for Android builds.

- Docs: <https://docs.unity3d.com/Packages/com.unity.xr.openxr@latest>
- OpenXR standard (background reading): <https://www.khronos.org/openxr/>

### XR Interaction Toolkit (`com.unity.xr.interaction.toolkit`)
Unity's interaction framework — the XR Origin rig, controllers, grabbing, teleporting,
UI rays. **This is the course's interaction layer.** Not optional.

It also ships the **XR Device Simulator** as an importable *sample* (Package Manager →
XR Interaction Toolkit → **Samples**), which drives a virtual headset and controllers
with the keyboard and mouse so you can test in Play mode without putting the headset on.
It works on Windows and macOS alike.

- Docs: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest>

### XR Hands (`com.unity.xr.hands`) — for hand tracking
Unity's vendor-neutral hand tracking, built on the OpenXR hand tracking extension. Add it
only when an activity calls for hands rather than controllers.

- Docs: <https://docs.unity3d.com/Packages/com.unity.xr.hands@latest>

### AR Foundation + Unity OpenXR: Meta — for passthrough and mixed reality
Passthrough, planes, and anchors on Quest 3 / 3S are reached through **AR Foundation**
(`com.unity.xr.arfoundation`) with the **Unity OpenXR: Meta** provider
(`com.unity.xr.meta-openxr`). This is the OpenXR route to mixed reality — no Meta SDK
required. Add these only for the mixed reality activities.

- AR Foundation: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest>
- Unity OpenXR: Meta: <https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@latest>

> **Why there is no Meta XR All-in-One SDK here.** Meta's SDK and its Building Blocks are
> a perfectly good way to build for Quest, and most tutorials online use them. This course
> uses OpenXR instead for three reasons: what you learn transfers to any headset rather
> than to one vendor's hardware; you assemble the rig yourself instead of dropping in a
> prefab, so you can see how it works; and the project stays small enough to live
> comfortably in Git. **Do not install the Meta XR SDK alongside these packages** —
> the two stacks compete over the same settings and the result is hard to debug. If you
> are following an online tutorial that starts with Building Blocks, it is for a different
> stack than ours.

**Setup walkthrough:** `OpenXR_Unity_Setup_Guide.pdf`

---

## 5. Headset and device tooling

### Supported headsets
**Meta Quest 2**, **Meta Quest 3**, **Meta Quest 3S**. All three run the same builds, but
they are not equally capable — Quest 3 and 3S share the newer Snapdragon XR2 Gen 2 chip,
while Quest 2 is a generation behind.

The difference that matters most for this course is **passthrough**: Quest 3 and 3S have
full-colour passthrough and support the Depth API, and Quest 2 has low-resolution
greyscale passthrough and no Depth API. Any activity involving mixed reality will look
substantially different on a Quest 2. Check which headset you have before starting the
XR weeks.

- Official device comparison: <https://developers.meta.com/horizon/resources/compare-devices/>
- Per-device performance and optimisation guidance: <https://developers.meta.com/horizon/resources/device-optimization-comparison/>

### Developer mode — already done for you
**The course headsets arrive with Developer Mode enabled.** You do not need a Meta
developer account, an organisation, or the Meta Horizon mobile app to work on this course.
The only thing you do yourself is accept the USB debugging prompt that appears inside the
headset the first time you connect it to a new computer.

If you are using your **own** headset instead, you will need to enable developer mode on
it yourself:

- Register a developer account: <https://developers.meta.com> — and create an
  **organisation** on the dashboard, which developer mode requires
- Meta Horizon mobile app, to pair the headset and turn on **Developer Mode**:
  <https://www.meta.com/quest/setup/>

### Meta Quest Developer Hub (MQDH)
Formerly Oculus Developer Hub, so you will still see it called **ODH** in older docs and
in Meta's own URLs — same tool. Deploy builds to the headset, mirror what the headset is
seeing onto your computer screen, record video and screenshots of your work, browse device
files, and read logs. **Works on both Windows and macOS** — this is the main device tool
on the course, and the only one Mac users get.

- Download and docs: <https://developers.meta.com/horizon/documentation/unity/ts-odh/>

### Cables, not Wi-Fi
Everything on this course is done **tethered over USB-C**. MQDH can run ADB over Wi-Fi and
Quest Link has a wireless Air Link mode, but campus wireless is not reliable enough for
either, and a connection that drops mid-deploy costs more time than the cable ever will.
Bring a USB-C cable that carries **data** — charge-only cables look identical and are a
common source of "my headset isn't detected".

### Meta Quest Link — **Windows only**
Runs your Unity scene live in the headset straight from Play mode. Requires the Meta
Horizon desktop app on Windows. Not taught on this course — the two workflows in the
OpenXR guide cover everything and work on every machine — but useful if you have a
Windows PC and want it.

- <https://www.meta.com/help/quest/link/>

### ADB (Android Debug Bridge)
Comes bundled with Unity's Android SDK module and with MQDH, so you usually don't install
it yourself. Useful when a build refuses to deploy.

- <https://developer.android.com/tools/adb>

---

## 6. Course repository

- Course materials and activities: this repository
- Supplied `.gitignore` for Unity projects: `Unity_Course.gitignore` in this folder

---

## Version summary — check yours matches

| Thing | Required |
|---|---|
| Unity Editor | 6.3 LTS (`6000.3.x`) |
| Unity modules | Android Build Support + OpenJDK + Android SDK & NDK Tools |
| Headset OS | Keep the headset updated via the Meta Horizon app |
| GitHub Desktop | Latest |

If a guide, activity, or lab machine disagrees with this table, this table wins — tell
your tutor so it can be corrected.
