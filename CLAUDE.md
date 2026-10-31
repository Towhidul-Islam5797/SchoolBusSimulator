@AGENTS.md

# School Bus Simulator

## Project
- Unity prototype of a school bus simulator for Fiverr client stryflex, built to pitch on Steam. PC only.
- Unity 6000.3.10f1, URP, new Input System (`Keyboard.current`). No legacy `Input` class.
- Git branch: TowhidRahat. Scene files use Git LFS.
- Developer: Rahat, a beginner game developer. Explain what code does and why in plain, simple language.

## Contract and milestones
- Total $2,200 over 60 days, four paid stages.
- Stage 1, driving system and bus setup: done and approved.
- Stage 2, lighting: delivered. Sprints 1 to 4 done (headlights, brake and reverse lights, turn signals, hazard lights). Cabin lights and LED vs incandescent were skipped by decision and can be revisited later.
- Stage 3, sign systems ($350, 9 Oct to 8 Nov 2026): ACTIVE.
- Stage 4, passenger gameplay, route, atmosphere, polish, optimization ($1,150): pending client funding. Do not start early.

## Stage 3 scope
- Build traffic lights and street signs, with their scripts.
- The stop arm and crossing arm are moved to a later milestone. Do not build them.
- The sign list and sign behavior are not yet decided with stryflex. Ask before assuming.
- Any premade sign asset needs client approval before use.

## Hard boundaries
- This is a prototype, not an MVP. Add nothing outside the current sprint, however small.
- Out of bounds until funded and active: passenger gameplay, route system, audio, day/night cycle, pre-trip UI, weather, multiplayer, AI traffic, mobile build.
- Do not rewrite or "improve" working code. Make single targeted changes only.

## Workflow
1. Read the relevant existing files first. Never assume what the code looks like.
2. Propose a plan: what changes, which files are touched, the done condition. Wait for explicit approval before writing anything.
3. Work one sprint at a time. Never move to the next sprint without confirmation.
4. Write a SprintDoc .txt for each sprint (goals, changes, done condition) in `SprintDocs/` at the repo root.
5. When a sprint is done, give a ready-to-copy Git commit message.
6. When a milestone is done, write a delivery checklist and list known debt carried forward.
7. Ask before starting if anything is unclear.

## Editor split
- By default, Claude writes scripts and Rahat places objects, creates materials, and wires Inspector fields. For each sprint, list the Editor steps Rahat must do.
- Claude may edit `.unity`, `.prefab`, `.mat` or `.meta` files only when Rahat explicitly asks for that specific task. Before editing, ask Rahat to save and close the scene or prefab in Unity, and never change a `.meta` GUID.

## Code conventions
- Scripts live in `Assets/V1.0/Scripts/<Folder>/`.
- Layout: `Update()` at the top, then one `#region` per sprint holding that sprint's fields and methods.
- Superseded sprint code stays commented inside its own region. The latest uncommented region is the active code. Always check it first.
- Tunable values are public Inspector fields, never hardcoded.
- Visual state uses an on/off material pair (for example `headlightOnMaterial` and `headlightOffMaterial`).
- Left and right are named from the driver's perspective.
- Keep code simple and readable. One clear responsibility per file and function. Clear names, no unnecessary comments, no advanced patterns, no deprecated Unity APIs.
- Prefer extending the existing central script (`BusLights.cs` pattern) over adding many small scripts, unless a new responsibility clearly needs its own file.

## Existing scripts
- `Bus/BusController.cs`: driving, gears R/N/D, steering, WheelColliders, wheel meshes.
- `Bus/BusLights.cs`: headlights, rear lights, turn signals, hazards. Reads `BusController.currentGear`.
- `Bus/SteeringWheelRotator.cs`: rotates the steering wheel from `currentSteer`.
- `Camera/CameraFollow.cs`: fixed-offset follow camera with speed-based FOV.
- `Speed/SpeedometerUI.cs`: shows `currentSpeed * 2` as MPH and the current gear.

## Controls
- W/S throttle and brake, A/D steer, Q shifts up (R to N to D), E shifts down.
- L headlights, Z left signal, C right signal, X hazards.

## Technical notes
- WheelColliders need +Z forward. The bus mesh faces +X, so `BusRoot` has zero rotation, the mesh is a child rotated -90 on Y, and Rigidbody and `BusController` sit on `BusRoot`.
- Physics is tuned in the Inspector, not in code. Do not hardcode WheelCollider values.
- Tuned values on `BusRoot`: mass 8500, angular damping 5, center of mass Y -1, motor force 2000, brake force 700, max speed 60.

## Known debt
- LOD Groups are unfinished on the Colorful City prefabs (about 55). Rahat does these manually.
- `BusSimpleMove.cs` is unused dead code, if still present.
- Per-wheel WheelCollider suspension and spring/damper values were never recorded.
- SprintDocs for Stage 1, Stage 2, Sprint 1 and Sprint 2 exist. Sprints 3 and 4 (turn signals, hazards) have no docs.

## Open questions
- Confirm Stage 3 scope with stryflex: traffic lights and street signs only, or also the stop and crossing arms.
- Confirm the sign list and the premade sign asset.
- None for SprintDocs: the folder is `SprintDocs/` at the repo root.

## Style for replies
- Short and direct. No filler, no emojis, no decorative formatting.
- Suggest one resource, doc page or concept to learn more when relevant.
