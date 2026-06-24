# Design Studio — An Interactive Design Thinking Simulation

An interactive 3D Unity simulation that teaches the five stages of the design thinking process — Empathize, Define, Ideate, Prototype, and Test — inside an explorable office "studio." Learners read what each stage means, see how the stages connect into one journey, and then practise what they've learned by matching real tasks to the right stage.

---

## GCGO Statement

My GCGO is **Education**.

**My mission** is to become a designer and developer who builds solutions that make hard or abstract things easier to understand and act on, and to work with organisations creating real impact for real people.

This project is a direct expression of that mission: I took an abstract problem-solving framework that is usually taught through slides and lectures, and turned it into something a person can pick up, click through, and practise — so the idea actually sticks.

---

## Problem Context

**The problem.** Most people try to solve problems by jumping straight to a solution before they've understood it. Design thinking is a well-proven framework for avoiding exactly that, but it's almost always taught abstractly: a diagram of five boxes, a lecture, a slide deck. It's easy to nod along to and just as easy to forget, because nothing about that format asks the learner to *do* anything.

**Why it matters.** The quality of what people build depends heavily on whether they understood the problem before building.  Teaching it in a way that's memorable and active, rather than passive, has a compounding effect on everything that learner goes on to create.

**How it connects to my GCGO.** This is an education tool first. It takes a transferable, high-leverage skill and makes it interactive, visual, and self-paced. That sits squarely inside the Education challenge: using technology to improve understanding and make a valuable concept genuinely learnable.

---

## Simulation Overview

**What it does.** The simulation drops the learner into a 3D office set up as a design studio. The five stages of design thinking are each represented by an object on the desk. The experience has two layers:

1. **Explore** — the learner clicks each of the five desk objects to open an information panel explaining what that stage of design thinking involves. A glowing line connects the five objects in order, so the learner sees design thinking as one connected journey rather than five disconnected steps.
2. **Practise** — a drag-and-drop matching activity. The learner drags task cards (sticky notes) onto the object representing the stage that task belongs to. Correct matches lock into place; wrong ones return to their starting spot. When all five are matched, the learner has demonstrated they understand what each stage actually means in practice.

**Target users.** Students and early-career builders — anyone new to design thinking or anyone who has heard of it but never practised it. Because it runs in a browser and on Android, it's accessible without any special hardware or installation barrier.

**Key interactions.**
- Click or tap a desk object to read about its design thinking stage.
- Drag a sticky note onto the object whose stage matches the task on the note.
- Tap a sticky note to read its text in an enlarged overlay.
- Complete an activity to trigger a confetti celebration and a completion message.

---

## Unity Mechanics Implemented

### UI

The interface is built on a Unity Canvas using the Input System's `InputSystemUIInputModule` on the EventSystem. An information panel (title, body text, and a close button) updates its contents whenever a stage is selected, driven by my `PanelController` script. A second overlay, managed by `NoteReader`, displays the full text of a sticky note when it's tapped, so the small text on the cards is always readable. Completion messages and the confetti win text are also UI elements that are hidden by default and switched on by script when the learner finishes an activity.

### Scripting

The project is built from a set of focused C# scripts, each with a single responsibility:

- **`PhaseData`** — a ScriptableObject holding each stage's name, description, and order (1–5). Storing the content as data means I can edit what each stage says without touching code.
- **`DesignPhaseObject`** — marks a desk object as belonging to a particular stage by holding a reference to its `PhaseData`.
- **`PhaseInteractor`** — casts a ray from the camera to detect which object the learner clicked or hovered, and opens the matching information panel.
- **`PanelController`** and **`NoteReader`** — show and hide the information panel and the enlarged note overlay.
- **`JourneyLine`** — positions the Line Renderer through the five objects.
- **`ProgressTracker`** — tracks how many stages have been viewed and triggers the win state.
- **`Note`** — holds which stage a sticky note belongs to and detects, through trigger callbacks, which object it's currently over.
- **`DragMatchManager`** — runs the whole matching game: picking up a note, dragging it, checking the drop, and firing the win reaction.

Across these I use a mix of public and private variables (data references, state flags like `placed`, counters like `correctCount`) and methods that respond both to Unity's lifecycle (`Awake`, `Update`) and to physics events (`OnTriggerEnter`, `OnTriggerExit`).

### Collision

Collision is the foundation of the matching game, implemented as trigger-based detection. Each sticky note has a **kinematic Rigidbody** and a **Box Collider set to Is Trigger**; each phase object has its own Box Collider. When a dragged note overlaps a phase object, Unity fires `OnTriggerEnter` on the note (it carries the Rigidbody), and the note records which `DesignPhaseObject` it's currently over. On release, `DragMatchManager` reads that recorded target and decides whether the match is correct. This is a genuine collision interaction — distinct from the raycasting — and it's what makes the drag-and-drop game work.

### Raycasting

Raycasting handles all selection. `PhaseInteractor` builds a ray from the camera through the pointer's screen position with `Camera.ScreenPointToRay`, then uses `Physics.Raycast` to find the object directly under the cursor — used both to open the right information panel on click and to highlight objects on hover. `DragMatchManager` uses the same technique to pick up a sticky note. Without raycasting, nothing in the scene would be clickable; it's the bridge between a 2D screen tap and a 3D object.

### Line Renderer

A Line Renderer, positioned by my `JourneyLine` script, connects the five phase objects in their correct order with a single continuous line. This visually reinforces the core teaching point: design thinking is a connected flow from Empathize through to Test, not five isolated boxes. The line is drawn in world space so it tracks the real positions of the desk objects.

---

## Additional Features

I implemented several features beyond the module scope:

1. **Audio system** — looping background music and a click sound effect, played through Audio Sources, to give the studio atmosphere and interaction feedback.
2. **Particle system** — a confetti burst that celebrates completing an activity, built from Unity's particle system.
3. **Drag-and-drop matching mini-game** — a full interactive activity, including projecting the pointer onto a plane so dragging works correctly on a fixed camera.
4. **Tap-to-read overlay** — tapping a note (as opposed to dragging it) opens an enlarged, readable version of its text, with logic that distinguishes a tap from a drag.
5. **Hover highlighting** — desk objects tint when the cursor is over them, giving the learner a clear affordance that they're interactive.
6. **Cross-platform input** — by reading input through `Pointer.current`, a single build responds to both a mouse click on the web and a finger tap on Android, with no separate control schemes and a fixed camera that keeps it fully mobile-friendly.

---

## Build Information

**WebGL deployment link:** https://donaldduru.itch.io/webed

**Android build (APK):** 

### Instructions for running the project

**Play it in a browser (easiest):** open the WebGL link above. Click objects to read each stage, then drag the sticky notes onto the matching objects.

**Play it on Android:** download the APK from the link above, allow installation from your browser/file manager if prompted, install, and open. Tap objects and drag notes the same way.

**Open the project from source (Unity):**
1. Clone this repository.
2. Open it with **Unity 6** (the version the project was built in).
3. The office environment asset (MarpaStudio) is excluded from this repository because of its size and licence, so import it from the Unity Asset Store before opening the scene, or the desk objects will be missing.
4. Open the scene in `Assets/Scenes`, press **Play**, and interact with the mouse.

---

## Project Structure

- **Scenes** — the main simulation scene lives in `Assets/Scenes`.
- **Scripts** — all gameplay scripts are in `Assets/Scripts`.
- **PhaseData** — the five ScriptableObject assets (Phase1_Empathize … Phase5_Test) that hold the stage content.
- **Prefabs** — the sticky note is set up once as a prefab and reused for all five notes.
- **Assets used** — MarpaStudio office environment, the Mini First Person Controller package (camera only; movement removed for mobile), and TextMeshPro for all text.