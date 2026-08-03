# Guides

**Semester 2, 2026**

This folder holds the course's **setup and workflow reference material**. It is separate
from the weekly activity folders on purpose: the weekly folders teach you how to *make*
things, and this folder gets your tools working so you can.

Between these three documents you should be able to work through any setup problem on the
course — installing software, getting a repository running, moving work between machines,
and getting a Unity project onto a Meta Quest headset on either Windows or macOS. If you
hit a setup problem that none of them answer, that's a gap in the guides — tell your tutor.

## The stack these guides assume

GitHub Desktop · Unity 6.3 LTS · OpenXR + XR Interaction Toolkit · Meta Quest 2 / 3 / 3S
· Windows and macOS

The course is **fully OpenXR** — Unity's own vendor-neutral packages, not the Meta XR SDK
and not Building Blocks. Online Quest tutorials mostly use Meta's stack; ours is
different on purpose, and mixing them causes problems. `Software_and_Frameworks.md`
explains why.

---

## Start here

**New to the course? Read them in this order.**

1. `Software_and_Frameworks.md` — install everything
2. `Unity_GitHub_Course_Guide_V1.pdf` — get your repository working
3. `OpenXR_Unity_Setup_Guide.pdf` — get Unity talking to a headset

---

## What's in this folder

### `Software_and_Frameworks.md`
**The single reference list of every piece of software, framework, and documentation link
used on this course.**

Covers the whole stack in install order — GitHub account and GitHub Desktop, Unity Hub
and the Unity 6.3 LTS Editor (including which modules to tick), a code editor, the XR
packages added inside Unity, and the Meta device tooling for Quest 2 / 3 / 3S. Each entry
gives the download link and the official documentation link. Ends with a version summary
table you can check your machine against.

This is a *reference*, not a walkthrough. It tells you what to install and where to get
it; the two guides below tell you how to use it.

### `Unity_GitHub_Course_Guide_V1.pdf`
**The full Unity + GitHub Desktop workflow guide.** Source: `Unity_GitHub_Course_Guide_V1.tex`.

Takes you from no GitHub account to a working, well-organised course repository:
creating the account, installing GitHub Desktop and Unity Hub, creating the course
repository, installing the supplied `.gitignore`, creating your first Unity project,
making your first commit, organising the repository folders, what to keep out of Git, the
daily commit/push workflow, moving between computers, and a troubleshooting section.

Read this before you create your Unity project, not after. The `.gitignore` has to be in
place first — putting it in later is much more painful than putting it in early.

### `OpenXR_Unity_Setup_Guide.pdf`
**The XR setup and workflow guide.** Source: `OpenXR_Unity_Setup_Guide.tex`.

Two halves. The first gets you from "I have a Unity project" to "my build runs on a
headset" — you read it once, and it ends by pointing you at Unity's VR template so you
never have to do it by hand again. The second covers the two ways you'll test your work
week to week, on the headset and in the XR Device Simulator — you'll come back to it all
semester.

### `Unity_Course.gitignore`
The `.gitignore` file supplied for the course. Copy it to the **root of your repository**
and rename it to exactly `.gitignore`. It ignores Unity's generated folders recursively,
so it works with several Unity projects nested anywhere inside the repository. Installing
it is covered step by step in the Unity + GitHub guide.
