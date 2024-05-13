## Introducction
Upon opening the project, my initial step is to engage with the gameplay to comprehend its mechanics thoroughly. Subsequently, I delve into analyzing the project's structure and its intercommunication among various components.

Key observations:

1. Communication predominantly relies on singletons and direct calls between components, resulting in interdependencies. To address this, I introduced a signal bus system using the "signal" library available at this Git repository [SIGNALS](https://github.com/yankooliveira/signals).
2. The absence of namespaces limits organization and clarity. Implementing namespaces would aid in segregating behaviors and functionalities effectively.
3. The absence of assemblies possibly hampers compilation speed. Incorporating assemblies might expedite the compilation process.
4. The use of standard Text instead of TextMeshPro impacts performance negatively. Considering the project's age, this choice is understandable but upgrading to TextMeshPro could enhance performance significantly.
5. UI elements' alignment with the camera is awkward due to a 45-degree rotation, making UI modification cumbersome.
6. The popup/view system relies on an enum and switch, which could be replaced with a state machine for easier scalability and maintenance.
7. Improved folder structuring with subfolders could enhance project organization and navigability.


## Tasks
### Feature 1: Player Collision
```
Tasks:
- Implement collision between the players so they can bounce off of each other during gameplay
- This new collision feature should be a fun and exciting addition to the gameplay 
- You can emphasize the collision with for example an animation and particle effects
```
I noticed that the brushes already had a collider on them, so I thought I could use it to create collisions. After investigating further, I realized that the power-ups were being collected in a certain way, so I used that same logic to push the player away when they collided with an object. I set it up so that the force of the push is determined by the size of the object that the player collides with.
![Collisions](https://github.com/antoniojesusnc/drawio/assets/2783284/02eb5cde-d864-4025-9e20-7eee0b9ba9f9)

### Feature 2: Skin Selection Screen
```
Tasks:
- Implement a new Skin Selection Screen (see mock-up 1)
-- The screen should contain a scrollable list with 12 different skins (using 2 models in 6 different colors)
-- All skins (in the scrollable list and top of the screen) should slowly rotate
-- Enhance the screen with for example additional animations and/or particle effects
- Replace the existing skin selection feature (present in the main menu) with a button that opens the Skin Selection Screen (see mock-up 2)
```
I created a new view called "SkinSelectorView". It has a preview and a list of all the skins in a scrollable grid. 
![SkinSelector](https://github.com/antoniojesusnc/drawio/assets/2783284/4c0b0094-652a-4f8f-a770-bedb2514b3a7)

### Feature 3: Daily Rewards
```
Tasks:
- Implement a Daily Rewards pop-up which appears every new day the user opens the game (see mock-up 3)
- Within the pop-up, the user can claim currency as a reward
- Make the claiming of currency feel satisfying by using for example animations and/or particle effects
- If the user does not open the game on consecutive days, the streak is broken and he returns to day 1
- You can enhance this screen with for example animations and/or particle effects
```
To implement the daily rewards feature, I created two new components - a view named "DailyRewardView" and a manager named "DailyRewardManager". The calculations for the daily reward are handled by the manager. The daily reward view will be displayed at the beginning of each day if the player has not yet claimed their reward.

![DailyReward](https://github.com/antoniojesusnc/drawio/assets/2783284/b97745ec-c0e2-4708-8bdd-d6f15a205bc6)

### Feature 4: Custom Feature
```
Tasks:
- Design and implement a new meta feature that you believe will have a strong positive impact on the engagement, retention, and/or monetization of Draw.io
- Add a text file called ‘custom_feature.txt’ in the root of the project, in which you explain the functionality of this custom feature.

```
As a custom feature, I have added a skin selector to the game which allows users to purchase some skins using the money obtained from the daily reward. I have also added different skins to make the game more attractive.
To use this feature, open the game and access the skin selector. You will see some skins with a cost displayed on them. You can only select these skins after purchasing them. Currently, the only way to obtain money is through the daily reward.
However, it would be ideal to add more ways for users to earn coins.
![Store](https://github.com/antoniojesusnc/drawio/assets/2783284/fcf240bc-d448-421f-b503-206f7acbc03e)

### Feature 5: Debug Menu
```
- Implement a Debug Menu that is accessible via the Main Menu
- Within this Debug Menu, it should be possible to individually enable/disable the 4 features that you have implemented previously.
- When every feature is disabled, the game should be in its original state.
```
To access the debug menu, you need to add the package from this git repository [MobileConsoleKit](https://github.com/pixeption/MobileConsoleKit). In this package, you can enable or disable a console log with various commands. To use it, simply press the log button at the bottom of the screen. Then, click on the wheel at the bottom to access the control flags.
With this console, you can easily send logs and add commands. You can find the scripts in the Console folder to see what has been added.

![Console](https://github.com/antoniojesusnc/drawio/assets/2783284/1ce02e82-97f0-4e48-8e06-8acb589d90e0)

## Conclusion
This is the test that I made. Firstly, thank you for the opportunity. Secondly, the test was interesting. Adding different views and functionality allowed me to showcase my skills. Although it may be a little lengthy, I believe it is still acceptable.
