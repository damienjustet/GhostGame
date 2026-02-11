# Ghost Game - Workflow Instructions

This repo is synced between GitHub and Anchorpoint (school server).

---

## 🏠 At Home (First Time Setup)

```powershell
git clone https://github.com/damienjustet/GhostGame.git
cd GhostGame
```

---

## 🏠 At Home (Daily Work)

```powershell
# 1. Get latest changes
git pull

# 2. Work on your project...

# 3. Save your changes
git add .
git commit -m "describe what you changed"
git push
```

---

## 🏫 At School (Sync Home → Anchorpoint)

```powershell
# 1. Pull your home changes into github-ghost
cd "c:\Users\justedam000\GHOST - VC\github-ghost"
git pull

# 2. Copy changed files to reinit-ghost
robocopy "c:\Users\justedam000\GHOST - VC\github-ghost\Ghost" "c:\Users\justedam000\GHOST - VC\reinit-ghost\Ghost" /E /XD .git Library Temp

# 3. Push to Anchorpoint
cd "c:\Users\justedam000\GHOST - VC\reinit-ghost"
git add .
git commit -m "changes from home"
git push
```

---

## 🏫 At School (Sync Anchorpoint → GitHub)

```powershell
# 1. Pull latest from Anchorpoint
cd "c:\Users\justedam000\GHOST - VC\reinit-ghost"
git pull

# 2. Copy to github-ghost
robocopy "c:\Users\justedam000\GHOST - VC\reinit-ghost\Ghost" "c:\Users\justedam000\GHOST - VC\github-ghost\Ghost" /E /XD .git Library Temp

# 3. Push to GitHub
cd "c:\Users\justedam000\GHOST - VC\github-ghost"
git add .
git commit -m "sync from anchorpoint"
git push
```

---

## 📁 Folder Reference

| Folder | Remote | Purpose |
|--------|--------|---------|
| `reinit-ghost` | Anchorpoint (school) | Team collaboration |
| `github-ghost` | GitHub | Bridge for home work |

---

## ⚠️ Important Notes

- The `robocopy` command excludes `.git`, `Library`, and `Temp` folders
- Always pull before starting work to avoid conflicts
- Never force push to Anchorpoint - it will break your team's repos
