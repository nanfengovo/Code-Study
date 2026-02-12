https://www.bilibili.com/video/BV1EJvPzUELq/?spm_id_from=333.337.search-card.all.click&vd_source=b7200d0eaee914e9c128dcabce5df118
* 首选单Agent，除了上下文太长

# 第一章：企业级大模型的部署
## 部署相关的问题/疑惑
* 为什么选择将dify/n8n安装在docker中但是ollama、Xinference不在docker中运行
	* 因为ollama需要使用到GPU而docker是使用cpu为主的，如果一定要在docker中运行需要安装其他的软件来支持GPU的调用
* 为什么不在ollama中安装嵌入模型（Embedding Models）和重排序（RerRank Models）模型
	* Ollama专注于简化大语言模型（LLM）的部署和管理，其核心优势在于LLM的易用性和快速上手
### 企业级大模型的场景
何为RAG :RETRIEVAL,
#### 基于RAG架构的开发
背景：
* 大模型的知识冻结
* 大模型幻觉
#### 基于Agent架构的开发
> 充分利用LLM的推理决策能力，通过增加规划、记忆和工具调用的能力，构造一个能够独立思考、逐步完成给定目标的智能体

![[Pasted image 20260212230925.png]]
