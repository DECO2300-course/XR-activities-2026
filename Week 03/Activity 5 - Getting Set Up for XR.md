# Activity 5: Getting Set Up for XR

A checkpoint, not a walkthrough. The steps live in the setup guide; this page tells you which
parts to do and what you should have at the end.

By the end of it you will be standing inside a world you can edit — change something in Unity,
build, and see it in front of your face.

## Objective

Create an XR project from Unity's **VR template**, get it running on a headset, and change
something in it.

## Prerequisites

- Everything under **Part 0 — Before you start** in the
  [OpenXR in Unity — Setup and Workflow Guide](../Guides/OpenXR_Unity_Setup_Guide.pdf)
- Access to a Meta Quest 2 / 3 / 3S. The course headsets are shared, so book your time now

## Instructions

Work through these sections of the guide, in order. Create a new project for this.

1. **Create the project** — guide **section 7**. In Unity Hub, choose the **VR** template. It
   arrives with the XR packages installed and a sample scene containing a working rig, which is
   where sections 3 to 6 of the guide would otherwise have got you by hand.
2. **Switch to Android** — guide **section 3**. The template does not know you are targeting a
   headset, so this part is still yours to do.
3. **Check the OpenXR settings** — guide **section 5**. In particular the **Meta Quest Support**
   feature group, the **Oculus Touch Controller Profile**, and **Project Validation**.
4. **Build and run it** — guide **section 9**. Put the headset on and look around the sample scene.
5. **Change something and build again** — see the extensions below. 
6. **Commit the project**, along with the project and package files the guide says belong in Git.

> **When something breaks.** Go to **Part 3 — When it goes wrong**. Section **13, Troubleshooting**
> is organised one symptom per heading; section **14, Where to get help** tells you what order to
> try things in when your symptom is not listed. Bring your tutor the exact error text and what
> you already tried.

## Understanding what the template did for you

The VR template is not magic, and it is worth knowing what it saved you: XR Plugin Management
installed, OpenXR enabled, the XR Interaction Toolkit and its Starter Assets imported, and a scene
with a camera rig, hands and controllers already wired to input actions.

Guide sections 3 to 6 build all of that by hand. You are not doing it that way, but read them when
something in the template looks wrong — they are the explanation of what each piece is for.

The one thing the template does *not* do is aim itself at a Quest. That is why steps 2 and 3 above
exist, and why **Project Validation** is worth running before every build: it catches the
mismatches that otherwise show up as a black screen in the headset.

## Extension Activities

The sample scene is a real Unity scene. Open it and treat it as yours.

### **Move the furniture**
Reposition, rescale or delete objects in the scene, then rebuild. Watch what a metre actually
feels like when you are standing in it rather than looking at it in the Scene view.

### **Change the look**
Swap materials and colours on the scene's objects, or change the skybox. Note how much stronger
lighting reads on a headset than on a monitor.

### **Add something of your own**
Drop in a primitive, give it a `Rigidbody` and a `Collider`, and rebuild. Can you pick it up?
Work out from the scene's existing objects what makes something grabbable and what does not.

### **Break it on purpose**
Move the floor, or move the rig, and see what that does to where you are standing. Undo it
afterwards. Knowing what a broken scene feels like from the inside is worth ten minutes.

## Outcome

A VR-template project that builds and runs on a headset, with at least one change you made
yourself, committed to your repository.

You also have a build pipeline you have used twice — so next time something goes wrong, you are
debugging your work rather than your setup.
