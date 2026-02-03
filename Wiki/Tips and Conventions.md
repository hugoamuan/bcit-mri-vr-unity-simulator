# Tips for Working on the Project
- The project has too many moving parts and sometimes it's very hard to track where things change in Unity or how they affect, so go slow and focus on learning and understanding only your part of the project. Trying to learn the whole project first before actually working on it is just unfeasible considering how large is the project
- Find out whether Unity already has an implementation or has a tool for what you have to implement
- To understand the project structure, check the [Structure Wiki page](https://bitbucket.org/vie_bcit/mri-vr-room4/wiki/Structure)

# Implement a New Feature Using BitBucket's Issues
1. Create an issue in the [Issues page](https://bitbucket.org/vie_bcit/mri-vr-room4/issues)
2. Create a branch name from the `dev` branch and develop the feature or fix the issue in that branch
  - Use the convention: `feature/ISSUE-issueNumber-title`
  - E.g. `feature/ISSUE-99-Head-Coil-Feedback-Report`
3. While working on the branch, for every commit created, remember to add `#ISSUENUMBER:` as prefix to all your commits in that issue
  - This is important because from a specific commit, you can click on the number and get redirected to that specific issue, and so, you can organize better the repository
  - E.g. `#99: Added template window for feedback report`
4. Submit a pull request to dev

# Tips for Working With Unity
- If you need to do any transformation on a game object that is fixed (is the default of that object), do it on import instead of in *Unity's Inspector*
- Use prefabs as much as you can, instead of just putting objects on the scene, as that makes it easier to test and isolate components
- Whenever you receive a `.fbx` with a full on 3D model, try to export and separate all its parts (prefabs, meshes and prefabs), as that makes the code more modular
- When trying to a 3D object as a Unity GameObject, instead of just plugging it into Unity inspector, create an empty GameObject and add the 3D mesh as a children object with a `Mesh` suffix
  - E.g. `Pillow` -> `PillowMesh`