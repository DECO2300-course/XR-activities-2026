# Activity 2: The Two Workflows

> **Headset badge: headset required.** The second half of this activity does not work without
> a Meta Quest 2 / 3 / 3S. The headsets are shared — book your time before you start.

## Objective

Activity 1 tested your scene in the **XR Interaction Simulator**, which is half of the loop you
will use for the rest of the course. This is the other half. Take the same scene to a real
headset, find out what the simulator could not tell you, and fix the worst of it.

## Prerequisites

- **[Activity 1](Activity%201%20-%20Your%20First%20XR%20Scene.md)** finished, saved and committed.
  Your scene runs in the **XR Interaction Simulator** and you have already grabbed the `Mug`
- The **Week 3 checkpoint** finished: [Getting Set Up for XR](../Week%2003/Activity%205%20-%20Getting%20Set%20Up%20for%20XR.md).
  You have built to a headset once already, so the twenty-minute first build is behind you
- **Packages you'll add this week:** none
- **Hardware:** a Meta Quest 2, 3 or 3S, charged, with paired controllers, and a **USB-C cable
  that carries data**. Charge-only cables look identical and will cost you the session

> **Your welfare comes first.** This activity puts you in a headset and asks you to move around
> and pay attention to how it feels. If you start to feel unwell — queasy, sweaty, headachy,
> oddly tired — **take the headset off straight away and stop**. Feeling sick is *data*, and
> writing it down is this activity working as intended. Nobody is expected to push through
> nausea, and nobody is marked down for stopping. Symptoms can arrive late, so if you feel off,
> leave the headset off for the rest of the session.

> **Mac users.** Everything here works on macOS. You cannot play to a headset from the Editor —
> that is Windows-only and this course does not teach it — so your device workflow is *build an
> `.apk` and deploy it*, which is what the instructions below describe for everybody.

## Instructions

### Step 1: Decide what you expect

Open `FirstXRScene.unity` and run it in the simulator once, to put it back in your head.

Now settle on an answer to each of these, before you build. Say them out loud, or to whoever is
sitting next to you:

- Is the table the right height for a standing person?
- Can a real arm reach the `Mug` without stepping forward?
- Will the scene hold framerate?
- Does anything about walking through that doorway worry you?

Guesses are fine, and being wrong is the useful outcome. The reason to answer first is that an
expectation is the only thing a surprise can contradict. Go in with no expectation and you will
put the headset on, look around, and conclude the scene was roughly what you imagined.

### Step 2: Build to the headset

Full instructions are **section 9** of
[the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md) — follow them there rather than
from memory. In short: connect the headset by USB-C, confirm there is no unanswered
authorisation prompt waiting inside it, then **File → Build Profiles** → **Android** →
**Build and Run**, with the build output going to a folder **outside your repository**.

Two things that catch people, both one line each:

- **`FirstXRScene` must be in the Scene List** in **File → Build Profiles**, and ticked. A scene
  that is not in the list is not in the build, and "black screen on launch" is very often just
  this
- If **Build and Run** cannot see your device, or the app builds but you cannot find it on the
  headset, both symptoms are in the guide's *Troubleshooting* section with their fixes

If Unity's **Build and Run** is being uncooperative, build the `.apk` on its own and drag it
onto **Meta Quest Developer Hub**'s device manager instead. Same result, and it works
identically on Windows and macOS.

### Step 3: Look at it properly

Put the headset on. Go looking for these four on purpose, and check each one against the answer
you gave in Step 1. The ones where you were wrong are the ones worth your attention.

**Scale.** Stand in front of the table. Is it desk height, or is it a coffee table? Walk to the
doorway — is it a door, or a gap you have to duck through? Compare against the numbers you typed
in Activity 1.

**Comfort.** Move the rig around the scene. Look up. Look at the floor. Turn quickly. Note
anything that makes you tense, blink, or want to stop — that feeling is data, and it is the only
instrument you have for it. If it goes past mild, stop.

**Reach.** Stand still, feet planted, and try to grab the `Mug` without stepping. Now try it
from where the player is most likely to actually be standing. Try picking it up from above and
from the side. Real shoulders have a much smaller envelope than a mouse does.

**Framerate.** Move your head from side to side reasonably fast and watch the edges of objects.
Judder — a stuttering, smearing quality to motion — is what dropped frames look like from
inside. Quest 2 targets 72 Hz and Quest 3 and 3S target 90 Hz by default. On a scene this small
you should see none at all. If you do, something is already wrong, and it is much cheaper to
find out now than in week 10.

### Step 4: Change the thing that was most wrong

Take the headset off and go straight back to the scene while it is still fresh.

Pick the **single** worst thing from Step 3 and fix it. Not all of it — one. Raise the table,
move the `Mug` closer, widen the doorway, whatever your own four answers pointed at. Then build
again and put the headset back on to check that the fix actually helped.

That second build is the part people skip, and it is where the loop earns its keep. A change
that looked obviously right in the Inspector is just another untested guess until you have stood
next to it.

Save and commit the scene.

> **Checkpoint.** One scene, both workflows, and a change you made because of something you
> could only have discovered by standing in it. Every week from here uses the same loop.

## Understanding why both workflows exist

They answer different questions, and neither one substitutes for the other.

| | **XR Interaction Simulator** | **On the headset** |
|---|---|---|
| Iteration speed | Seconds | Minutes |
| Headset needed | No | Yes |
| Real performance | No | Yes |
| Real scale, comfort, reach | No | Yes |
| Windows and macOS | Yes | Yes |
| Answers | *Does it work?* | *Is it any good?* |

The **XR Interaction Simulator** is where most of your hours should go. Logic, layout, wiring,
does-the-thing-do-the-thing — all of it iterates in seconds, needs no equipment, and can be done
anywhere. Booking a headset to test whether you assigned a reference correctly is a waste of a
headset and of your afternoon.

But it is a *model* of a headset, and every model leaves things out. It is not wrong about
scale, comfort, reach and framerate; it is **silent** about them. Your Editor renders one view
on a desktop GPU; the headset renders two at 72 or 90 frames a second on a mobile chip. Your
mouse has no shoulder. Your monitor has no inner ear. None of the four things you just wrote
down were discoverable from where you were sitting an hour ago, and none of them are small: a
scene at the wrong scale, or one that makes people queasy, is not a scene with a bug in it. It
is a scene that does not work.

Hence the loop:

1. **Build it in the XR Interaction Simulator.** Most of the week lives here
2. **Confirm it on the headset before you call it done.** Every session, not just before a
   deadline. Finding out on Wednesday that Wednesday's work is uncomfortable costs you
   Wednesday; finding out in week 10 costs you the project

**Work that has only ever run in the XR Interaction Simulator is not finished work.**

The reasoning, the full comparison, and the driving controls are all in **Part 2** of
[the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md). Keep it open beside you.

## Extension Activities

### **Break the framerate on purpose**

A scene this small will never stutter, so you have no idea where the ceiling is. Find it.

Logic: duplicate a lit, shadow-casting object until motion visibly degrades, doubling each time
— 50, 100, 200, 400 — rebuilding to the headset at each step. Note the number where judder
starts. Do the same in the **XR Interaction Simulator** and note that the Editor sails past it
without complaint.

Use `Ctrl+D` (`Cmd+D` on macOS) to duplicate, and keep the copies under one parent so you can
disable them all at once.

### **The blind scale test**

Build three copies of the doorway at 1.8 m, 2.0 m and 2.2 m, place them side by side, and have
someone else put them in order — without telling them the numbers.

Then swap and try it yourself. People are good at this from inside a headset and bad at it on a
monitor, which is the argument of this activity in one experiment.

### **An on-device frame counter**

"It felt smooth" is a weak measurement. Put a number in the scene.

Logic: on a world-space canvas parented near the table (**not** parented to the camera —
Activity 1 explains why), display a smoothed frames-per-second figure.

```csharp
// In Update(), on a script holding a TMP_Text reference:
smoothed = Mathf.Lerp(smoothed, 1f / Time.unscaledDeltaTime, 0.1f);
label.text = $"{smoothed:0} fps";
```

`Time.unscaledDeltaTime` rather than `Time.deltaTime`, so a changed time scale cannot flatter
your reading. Rebuild, and compare the number against your line 4.

### **Hand your build to someone else**

Build the `.apk`, deploy it to a different person's headset with **Meta Quest Developer Hub**,
and watch them use it — with casting on, so you can see what they see on your screen.

Watch where they reach, not where you told them to. The first person who is not you is the most
informative test in this week, and it costs about five minutes.

## Outcome

One scene, run through both workflows in one sitting, and improved because of it. In your
repository you now have a `FirstXRScene.unity` that has been stood in, judged against a real
body, and corrected.

The habit is the real outcome. Iterate in the **XR Interaction Simulator** because it is fast;
finish on the headset because it is true.

## Headset checkpoint

Before you close this week: your scene launched standalone on a Meta Quest 2 / 3 / 3S, the floor
was under your feet rather than at your knees, you picked the `Mug` up with a real controller and
threw it, and you changed something in the scene because of how it felt to be inside it. If any
of those is not true, this is the week to say so — from here on, every activity assumes this loop
works.

## References

- XR Interaction Simulator: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/xr-interaction-simulator-overview.html>
- Meta device comparison and refresh rates: <https://developers.meta.com/horizon/resources/compare-devices/>
