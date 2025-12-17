```bash
# 全局配置（所有仓库生效）
git config --global user.name "你的用户名"
git config --global user.email "你的邮箱@xxx.com"

# 验证配置
git config --list
```
#### 场景 1：本地新建项目，初始化 Git 仓库


```bash
# 1. 进入项目文件夹（工作区）
cd /你的项目路径/xxx-project

# 2. 初始化 Git 仓库（生成 .git 文件夹）
git init

# 3. 将工作区文件添加到暂存区（. 表示所有文件，也可指定单个文件：git add 文件名）
git add .

# 4. 提交暂存区的修改到版本库（-m 后写提交说明，必须有）
git commit -m "首次提交：初始化项目结构"

# 5. 查看提交记录（验证是否成功）
git log  # 简化版：git log --oneline
```

#### 场景 2：克隆远程仓库到本地（如从 GitHub 拉取项目）


```bash
# 方式1：HTTPS 地址（需输入账号密码/令牌）
git clone https://github.com/用户名/仓库名.git

# 方式2：SSH 地址（免密，需先配置 SSH 密钥）
git clone git@github.com:用户名/仓库名.git

# 克隆后进入项目目录
cd 仓库名
```
#### 场景 3：本地修改代码后，提交并推送到远程仓库

```bash
# 1. 查看工作区修改（可选，确认修改内容）
git status

# 2. 添加修改到暂存区
git add .  # 或指定文件：git add src/xxx.cs

# 3. 提交到本地版本库
git commit -m "修复：登录功能bug；新增：用户列表页"

# 4. 拉取远程最新代码（避免冲突，先拉再推）
git pull origin main  # main 是远程分支名（旧版是 master）

# 5. 推送到远程仓库
git push origin main
```

#### 场景 4：分支操作（开发新功能 / 修复 bug）

```bash
# 1. 查看所有分支（* 表示当前分支）
git branch

# 2. 创建并切换到新分支（如开发新功能：feature/user-info）
git checkout -b feature/user-info  # 等价于：git branch feature/user-info + git checkout feature/user-info

# 3. 在新分支开发后，提交代码（同场景3的 add/commit）
git add .
git commit -m "新增：用户信息编辑功能"

# 4. 切换回主分支
git checkout main

# 5. 将新分支合并到主分支
git merge feature/user-info

# 6. 推送主分支到远程
git push origin main

# 7. （可选）删除本地分支（功能开发完成后）
git branch -d feature/user-info
```

#### 场景 5：撤销 / 回滚操作（新手救星）

```bash
# 1. 撤销工作区的修改（未 add 的文件）
git checkout -- 文件名  # 恢复到最近一次 commit 状态

# 2. 撤销暂存区的修改（已 add 但未 commit）
git reset HEAD 文件名  # 回到工作区；git reset HEAD . 撤销所有暂存

# 3. 回滚到指定版本（已 commit 的修改）
git log  # 找到要回滚的版本号（前7位即可）
git reset --hard 版本号  # 谨慎使用！会删除后续所有修改

# 4. 放弃本地所有未提交的修改（回到远程最新状态）
git fetch --all
git reset --hard origin/main
```