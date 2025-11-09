// ARQUIVO: chamado.js (VERSÃO CORRIGIDA)

const ChamadoApp = {
    // --- 1. Propriedades (Estado) ---
    API_BASE_URL: 'http://localhost:5147/api',
    SERVER_ROOT_URL: 'http://localhost:5147', // URL base para links estáticos
    token: null,
    userInfo: {
        id: null,
        role: null,
        name: null
    },
    chamadoId: null,
    currentChamado: null, // Armazena o ViewModel
    hasEditPermission: false,

    // --- 2. Elementos DOM (Views) ---
    elements: {
        // Layout
        sidebarNav: document.getElementById('sidebar-nav'),
        userAvatar: document.getElementById('user-avatar'),
        userName: document.getElementById('user-name'),
        userRole: document.getElementById('user-role'),
        // Header
        elTitulo: document.getElementById('chamado-titulo'),
        elRequerente: document.getElementById('chamado-requerente'),
        elData: document.getElementById('chamado-data'),
        elStatusBadge: document.getElementById('chamado-status-badge'),
        // Form Detalhes
        formDetalhes: document.getElementById('form-detalhes-chamado'),
        formActionsFooter: document.getElementById('form-actions-footer'),
        messageContainer: document.getElementById('message-container'),
        // Campos do Form
        fieldAssunto: document.getElementById('assunto'),
        fieldDescricao: document.getElementById('descricao'),
        fieldTipo: document.getElementById('tipo'),
        fieldCategoria: document.getElementById('categoria'),
        fieldPrioridade: document.getElementById('prioridade'),
        fieldStatus: document.getElementById('status'),
        groupPrioridade: document.getElementById('group-prioridade'),
        groupStatus: document.getElementById('group-status'),
        // Seções
        tecnicoListContainer: document.getElementById('tecnico-list-container'),
        anexoListContainer: document.getElementById('anexo-list-container'),
        // Form Chat
        formNovaMensagem: document.getElementById('form-nova-mensagem'),
        inputNovaMensagem: document.getElementById('nova-mensagem'),
        elConversaContainer: document.getElementById('conversa-container')
    },

    // --- 3. Métodos (Controladores) ---

    init: async function() {
        this.token = localStorage.getItem('jwt_token');
        if (!this.token) {
            alert("Acesso não autorizado. Por favor, faça o login.");
            window.location.href = 'login.html';
            return;
        }

        // Define a URL raiz dinamicamente a partir da URL da API
        this.SERVER_ROOT_URL = this.API_BASE_URL.replace('/api', '');

        this.userInfo = this.decodificarToken(this.token);
        if (!this.userInfo) return; 

        this.chamadoId = new URLSearchParams(window.location.search).get('id');
        if (!this.chamadoId) {
            alert("ID do chamado não fornecido.");
            window.location.href = 'dashboard.html';
            return;
        }

        this.renderLayout();
        this.attachEventListeners();
        
        await Promise.all([
            this.popularDropdowns(),
            this.carregarChamado()
        ]);
        
        this.applyPermissions();

        feather.replace();
    },

    decodificarToken: function(token) {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const roleClaimKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            const userRole = payload[roleClaimKey] || payload.role; 
            if (!userRole) throw new Error("Token inválido: Role (perfil) não encontrado.");
            return {
                id: parseInt(payload.nameid),
                role: userRole,
                name: payload.sub
            };
        } catch (e) {
            console.error("Erro ao decodificar token:", e);
            localStorage.removeItem('jwt_token');
            window.location.href = 'login.html';
            return null;
        }
    },

    renderLayout: function() {
        const { role, name } = this.userInfo;
        const { sidebarNav, userAvatar, userName, userRole } = this.elements;
        userName.textContent = name;
        userRole.textContent = role;
        userAvatar.textContent = name.substring(0, 2).toUpperCase();

        const sidebarLinks = [
            { text: 'Dashboard', href: 'dashboard.html', icon: 'home', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Gerenciar Usuários', href: 'gerenciar_usuarios.html', icon: 'users', roles: ['Administrador'] },
            { text: 'Relatórios', href: 'relatorio.html', icon: 'bar-chart-2', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Configurações', href: 'configuracoes.html', icon: 'settings', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'FAQ', href: 'FAQ.html', icon: 'help-circle', roles: ['Administrador', 'Tecnico', 'Colaborador'] }
        ];
        
        const ul = document.createElement('ul');
        const currentPage = window.location.pathname.split('/').pop();

        sidebarLinks.filter(link => link.roles.includes(role)).forEach(link => {
            const li = document.createElement('li');
            const isActive = (link.href === currentPage) ? 'class="active"' : '';
            li.innerHTML = `<a href="${link.href}" ${isActive}><i data-feather="${link.icon}"></i> ${link.text}</a>`;
            ul.appendChild(li);
        });
        sidebarNav.innerHTML = '';
        sidebarNav.appendChild(ul);
    },

    carregarChamado: async function() {
        try {
            const response = await fetch(`${this.API_BASE_URL}/chamados/${this.chamadoId}`, {
                headers: { 'Authorization': `Bearer ${this.token}` }
            });

            if (!response.ok) {
                const errorText = await response.text();
                if (response.status === 403 || response.status === 401) {
                    alert("Você não tem permissão para ver este chamado.");
                    window.location.href = 'dashboard.html';
                    return;
                }
                throw new Error(errorText || "Falha ao carregar o chamado.");
            }

            this.currentChamado = await response.json();
            const chamado = this.currentChamado;

            this.elements.elTitulo.textContent = `#${chamado.id} - ${chamado.titulo}`;
            this.elements.elRequerente.textContent = chamado.usuario.nome;
            this.elements.elData.textContent = new Date(chamado.dataAbertura).toLocaleString();
            this.elements.elStatusBadge.textContent = chamado.statusChamado.nome;
            this.elements.elStatusBadge.className = `status status-${chamado.statusChamado.nome.toLowerCase().replace(' ', '-')}`;

            this.elements.fieldAssunto.value = chamado.titulo;
            this.elements.fieldDescricao.value = chamado.descricao;
            this.elements.fieldTipo.value = chamado.idTipo;
            this.elements.fieldCategoria.value = chamado.idCategoria;
            this.elements.fieldPrioridade.value = chamado.idPrioridade;
            this.elements.fieldStatus.value = chamado.idStatus;
            
            this.elements.elConversaContainer.innerHTML = ''; 
            this.adicionarMensagemAoChat(
                chamado.usuario.nome, 
                chamado.descricao, 
                new Date(chamado.dataAbertura),
                "Descrição Original"
            );
            chamado.mensagens.forEach(msg => {
                this.adicionarMensagemAoChat(msg.autor.nome, msg.conteudo, new Date(msg.dataEnvio));
            });

            this.renderTecnicos(chamado.tecnicosAtribuidos);
            this.renderAnexos(chamado.anexos);

        } catch (error) {
            this.elements.elTitulo.textContent = "Erro ao carregar chamado.";
            this.elements.messageContainer.innerHTML = `<p class="error-text">${error.message}</p>`; 
            console.error(error);
        }
    },

    applyPermissions: function() {
        if (!this.currentChamado) return;
        const { role, id } = this.userInfo;
        const isOwner = (this.currentChamado.idUsuario === id);
        this.hasEditPermission = (role === 'Administrador' || role === 'Tecnico' || (role === 'Colaborador' && isOwner));
        const isPrivileged = (role === 'Administrador' || role === 'Tecnico');
        this.elements.fieldAssunto.disabled = !this.hasEditPermission;
        this.elements.fieldDescricao.disabled = !this.hasEditPermission;
        this.elements.fieldTipo.disabled = !this.hasEditPermission;
        this.elements.fieldCategoria.disabled = !this.hasEditPermission;
        this.elements.fieldPrioridade.disabled = !isPrivileged;
        this.elements.fieldStatus.disabled = !isPrivileged;
        this.elements.groupPrioridade.style.display = isPrivileged ? 'block' : 'none';
        this.elements.groupStatus.style.display = isPrivileged ? 'block' : 'none';
        if (this.hasEditPermission) {
            this.elements.formActionsFooter.style.display = 'flex';
        }
    },

    popularDropdowns: async function() {
        const fetchDropdown = async (endpoint, selectId) => {
            const selectEl = document.getElementById(selectId);
            try {
                const response = await fetch(`${this.API_BASE_URL}/${endpoint}`, {
                    headers: { 'Authorization': `Bearer ${this.token}` }
                });
                if (!response.ok) throw new Error(`Erro ao buscar ${endpoint}`);
                const data = await response.json();
                selectEl.innerHTML = ''; 
                const defaultOption = document.createElement('option');
                defaultOption.value = "";
                defaultOption.textContent = "Selecione...";
                defaultOption.disabled = true;
                selectEl.appendChild(defaultOption);
                data.forEach(item => {
                    const option = document.createElement('option');
                    option.value = item.id;
                    option.textContent = item.nome;
                    selectEl.appendChild(option);
                });
            } catch (error) {
                console.error(error);
                selectEl.innerHTML = `<option value="">Erro ao carregar</option>`;
            }
        };
        await Promise.all([
            fetchDropdown('tipos', 'tipo'),
            fetchDropdown('categorias', 'categoria'),
            fetchDropdown('prioridades', 'prioridade'),
            fetchDropdown('status', 'status')
        ]);
    },

    renderTecnicos: function(tecnicos) {
        const container = this.elements.tecnicoListContainer;
        container.innerHTML = ''; 
        if (tecnicos.length === 0) {
            container.innerHTML = '<p class="text-muted">Nenhum técnico atribuído a este chamado.</p>';
            return;
        }
        const ul = document.createElement('ul');
        ul.className = 'tecnico-list-items';
        tecnicos.forEach(tec => {
            const li = document.createElement('li');
            li.innerHTML = `
                <div class="avatar">${tec.nome.substring(0, 2).toUpperCase()}</div>
                <span>${tec.nome}</span>
            `;
            ul.appendChild(li);
        });
        container.appendChild(ul);
    },

    renderAnexos: function(anexos) {
        const container = this.elements.anexoListContainer;
        container.innerHTML = ''; 
          if (anexos.length === 0) {
            container.innerHTML = '<p class="text-muted">Nenhum anexo encontrado para este chamado.</p>';
            return;
        }
        const ul = document.createElement('ul');
        ul.className = 'anexo-list-items';
        
        anexos.forEach(anexo => {
            const li = document.createElement('li');
            li.className = 'anexo-item';

            // --- INÍCIO DA CORREÇÃO ---
            // Cria a URL completa apontando para o servidor (ex: http://localhost:5147/uploads/arquivo.pdf)
            const urlCompleta = `${this.SERVER_ROOT_URL}/${anexo.caminhoArquivo}`;
            // --- FIM DA CORREÇÃO ---

            li.innerHTML = `
                <i data-feather="file-text"></i>
                <span>${anexo.nomeArquivo} (${(anexo.tamanhoBytes / 1024).toFixed(1)} KB)</span>
                <a href="${urlCompleta}" target="_blank" title="Baixar" class="btn-download-anexo">
                    <i data-feather="download"></i>
                </a>
            `;
            ul.appendChild(li);
        });
        container.appendChild(ul);
        feather.replace();
    },

    adicionarMensagemAoChat: function(autor, conteudo, data, titulo = null) {
        const item = document.createElement('div');
        item.className = 'conversa-item';
        const headerExtra = titulo 
            ? `<span><strong>(${titulo})</strong></span>`
            : `<span>${data.toLocaleString()}</span>`;
        item.innerHTML = `
            <div class="avatar">${autor.substring(0, 2).toUpperCase()}</div>
            <div class="conversa-conteudo">
                <div class="conversa-conteudo-header">
                    <strong>${autor}</strong>
                    ${headerExtra}
                </div>
                <p>${conteudo.replace(/\n/g, '<br>')}</p>
            </div>
        `;
        this.elements.elConversaContainer.appendChild(item);
    },

    attachEventListeners: function() {
        this.elements.formNovaMensagem.addEventListener('submit', (event) => {
            event.preventDefault();
            this.enviarNovaMensagem();
        });
        this.elements.formDetalhes.addEventListener('submit', (event) => {
            event.preventDefault();
            this.enviarEdicaoChamado();
        });
    },

    enviarEdicaoChamado: async function() {
        const { role } = this.userInfo;
        const msg = this.elements.messageContainer;
        msg.innerHTML = '';
        const dadosChamado = {
            titulo: this.elements.fieldAssunto.value,
            descricao: this.elements.fieldDescricao.value,
            idTipo: parseInt(this.elements.fieldTipo.value),
            idCategoria: parseInt(this.elements.fieldCategoria.value),
            idPrioridade: (role === 'Administrador' || role === 'Tecnico') ? parseInt(this.elements.fieldPrioridade.value) : null,
            idStatus: (role === 'Administrador' || role === 'Tecnico') ? parseInt(this.elements.fieldStatus.value) : null,
        };
        if (!dadosChamado.titulo || dadosChamado.titulo.trim() === "") {
             msg.innerHTML = '<p class="error-text">O campo Assunto é obrigatório.</p>';
             return;
        }
        if (!dadosChamado.descricao || dadosChamado.descricao.trim() === "") {
             msg.innerHTML = '<p class="error-text">O campo Descrição é obrigatório.</p>';
             return;
        }
        if (isNaN(dadosChamado.idTipo)) {
             msg.innerHTML = '<p class="error-text">O campo Tipo é obrigatório.</p>';
             return;
        }
        if (isNaN(dadosChamado.idCategoria)) {
             msg.innerHTML = '<p class="error-text">O campo Categoria é obrigatório.</p>';
             return;
        }
        if (dadosChamado.idPrioridade === null || isNaN(dadosChamado.idPrioridade)) {
            delete dadosChamado.idPrioridade;
        }
        if (dadosChamado.idStatus === null || isNaN(dadosChamado.idStatus)) {
            delete dadosChamado.idStatus;
        }
        try {
            const response = await fetch(`${this.API_BASE_URL}/chamados/${this.chamadoId}`, {
                method: 'PUT',
                headers: { 
                    'Authorization': `Bearer ${this.token}`,
                    'Content-Type': 'application/json' 
                },
                body: JSON.stringify(dadosChamado)
            });
            if (response.ok) {
                msg.innerHTML = '<p class="success-text">Chamado atualizado com sucesso!</p>';
                await this.carregarChamado();
            } else {
                const erro = await response.json();
                if (erro && erro.errors) {
                    let errorMsg = Object.values(erro.errors).flat().join(' ');
                    throw new Error(errorMsg);
                }
                throw new Error(erro.title || 'Falha ao atualizar o chamado.');
            }
        } catch (error) {
            msg.innerHTML = `<p class="error-text">${error.message}</p>`;
            console.error(error);
        }
    },

    enviarNovaMensagem: async function() {
        const conteudo = this.elements.inputNovaMensagem.value.trim();
        if (conteudo === '') return;
        try {
            const response = await fetch(`${this.API_BASE_URL}/chamados/${this.chamadoId}/mensagem`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${this.token}`
                },
                body: JSON.stringify({ conteudo: conteudo })
            });
            if (response.ok) {
                this.elements.inputNovaMensagem.value = ''; 
                const novaMensagem = await response.json(); 
                this.adicionarMensagemAoChat(
                    novaMensagem.autor.nome,
                    novaMensagem.conteudo, 
                    new Date(novaMensagem.dataEnvio)
                );
            } else {
                throw new Error("Falha ao enviar mensagem.");
            }
        } catch (error) {
            alert(error.message);
            console.error(error);
        }
    }
};

// --- Ponto de Entrada ---
document.addEventListener('DOMContentLoaded', () => {
    ChamadoApp.init();
});