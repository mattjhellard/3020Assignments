# 3020 Assignments Repository
**_Since we've agreed on VS, this will be oriented for that context_**
## Suggested git group protocols:
1. Make sure to pull or at least fetch before making changes to a branch that isn't solely yours, especially before pushing those changes
2. Create a branch for yourself to work in, preferably just call it your name for maximum clarity, merge it with main/master at the end of sessions or when you've made major additions you want others to see immediately, this will more adequately address conflicts, particularly in VS which has a pretty nice merge conflict handling menu.
3. If a conflict arises because multiple members have developed the same thing, first check if the implementation is essentially the same, if it is, then whichever version has the best documentation should be kept, if they have equal documentation quality then the member performing the merge should relinguish their implementation, as the other was technically first. If the implementations are significantly different then talk to the other member and mutually decide which is the one that should be kept.
<br>
<br>

#### READ THIS IF TOTALLY UNCOMFORTABLE WITH BASIC GIT FUNCTIONS
- To work with the github repo in VS, you must first clone it. (Git > Clone Repository > use ...github.com/mattjhellard/3020Assignments), this will create a local copy of the repo
- If you don't have Git Changes pinned to the RHS of your VS window you can find it under the View tab on top, I suggest pinning it for convenience
- To import changes made to the github repo since cloning, you can pull (in Git Changes)
- You can preview changes made to the github repo from VS without pulling by fetching first
- To make changes to the github repo from VS, you should sign into your github account within VS (File > Account Settings > All Accounts > Add)
- When signed in, you can update the github repo by commiting changes and then pushing them. (in Git Changes)
- For major and/or experimental changes, you can create a new branch to push changes without worrying about effecting the main (aka master) branch. (Git Changes > branch selection dropdown > New Branch)
- Branches can be "merged", this creates a new commit that points to the source branches, the merged commit will show all changes from both branches, if changes conflict between branches (changes to same file in both branches), VS has a built-in conflict handler that will, when a merge is in progress with conflicts, allow you to pick and choose which portions of the file to keep from the head of each branch. (It also uses some presumably sophisticated logic to determine which portions of the file are actually conflicting)
- If you have any questions feel free to ask on the discord, but honestly the internet might be a faster better choice depending on how advanced the question is

