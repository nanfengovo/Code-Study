# Claude Code
## 常见命令
```
* /init
	1、初始化Claude.md 文件 记录项目背景需求和约束、
* /memory
	1、查看和管理个人/项目的通用记忆，即管理Claude.md文件的内容
	2、可以在terminal中使用#开头快速调用
	3、每次在开发过程中遇见了认为可以通用约束的内容就加进来，可以每次改完后和claude说让他自己加即可
* /add-dir
	1、将指定目录下的所有文件都添加到Claude的上下文中，实现多项目联动
* /clear
	1、清空当前会话的上下文，开始一个新任务
	2、每次完成一个任务就使用一下这个命令，不要上下文过大，避免自动压缩
	3、不要担心丢失信息，如果是重要信息应该写在Claude.md里面，毕竟一个任务是干一个任务的事情
* /compact
	1、压缩当前对话的上下文，保留要点，继续当前任务
	2、基本不会主动使用，因为压缩会丢失细节信息，影响后续响应的质量                    
	3、如果一个任务太大了建议应该拆分任务
* /mcp
	1、控制和管理MCP
	2、最常用的MCP有Context7和chrom-devtools-mcp
* /agent
	1、查看和管理sub agent的使用
	2、可以使用Github上的Agent 仓库里面定义好的Agent
	3、最常用的SubAgent有/code-review-ai:ai-review,代码审查
* /hooks
	1、查看和管理Claude Code Hooks
	2、例如每次代码完成后自动格式化
* /tasks
	1、列出和管理后台任务，可以使用！快速触发
* /export
	1、导出当前session的内容*** *	
*
```

# Codex
# Gemini
# Open Code
