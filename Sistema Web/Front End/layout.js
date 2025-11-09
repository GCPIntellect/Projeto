// ARQUIVO: layout.js
// Este script cuidará de carregar a sidebar e o perfil do usuário
// em todas as páginas "estáticas" (Configurações, FAQ, Relatórios, etc.).

const LayoutApp = {
    // --- 1. Propriedades (Estado) ---
    API_BASE_URL: 'http://localhost:5147/api',
    token: null,
    userInfo: {
        id: null,
        role: null,
        name: null
    },

    // --- 2. Elementos DOM (Views) ---
    elements: {
        sidebarNav: document.getElementById('sidebar-nav'),
        userAvatar: document.getElementById('user-avatar'),
        userName: document.getElementById('user-name'),
        userRole: document.getElementById('user-role'),
        menuToggle: document.querySelector('.menu-toggle'),
        sidebar: document.querySelector('.sidebar')
    },

    // --- 3. Métodos (Controladores) ---

    /**
     * Ponto de entrada principal.
     */
    init: function() {
        this.token = localStorage.getItem('jwt_token');
        if (!this.token) {
            alert("Acesso não autorizado. Por favor, faça o login.");
            window.location.href = 'login.html';
            return;
        }

        this.userInfo = this.decodificarToken(this.token);
        if (!this.userInfo) return; // Redirecionamento já feito

        this.renderLayout();
        this.attachEventListeners();
        
        // Renderiza ícones estáticos (como o do menu mobile)
        feather.replace(); 
        
        // Adiciona o botão de sair
        this.renderLogoutButton();
    },

    /**
     * Adiciona o botão de sair ao layout
     */
    renderLogoutButton: function() {
        const logoutButton = document.createElement('button');
        logoutButton.className = 'btn-logout';
        logoutButton.innerHTML = '<i data-feather="log-out"></i> Sair';
        
        // Adiciona ao sidebar
        this.elements.sidebar.appendChild(logoutButton);
        
        // Atualiza os ícones
        feather.replace();
        
        // Adiciona evento de click
        logoutButton.addEventListener('click', () => {
            localStorage.removeItem('jwt_token');
            window.location.href = 'login.html';
        });
    },

    /**
     * Decodifica o token JWT para obter os dados do usuário.
     */
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

    /**
     * Renderiza a Sidebar e o Perfil do Usuário.
     */
    renderLayout: function() {
        // Garante que os elementos existem antes de tentar usá-los
        if (!this.elements.sidebarNav || !this.elements.userAvatar || !this.elements.userName || !this.elements.userRole) {
            console.error("Elementos de layout (sidebar/perfil) não encontrados no HTML. Verifique os IDs.");
            return;
        }
        
        const { role, name } = this.userInfo;
        const { sidebarNav, userAvatar, userName, userRole } = this.elements;

        // 1. Renderizar Perfil do Usuário
        userName.textContent = name;
        userRole.textContent = role;
        userAvatar.textContent = name.substring(0, 2).toUpperCase();

        // 2. Renderizar Sidebar Dinâmica (com FAQ por último)
        const sidebarLinks = [
            { text: 'Dashboard', href: 'dashboard.html', icon: 'home', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Gerenciar Usuários', href: 'gerenciar_usuarios.html', icon: 'users', roles: ['Administrador'] },
            { text: 'Relatórios', href: 'relatorio.html', icon: 'bar-chart-2', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Configurações', href: 'configuracoes.html', icon: 'settings', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'FAQ', href: 'FAQ.html', icon: 'help-circle', roles: ['Administrador', 'Tecnico', 'Colaborador'] }
        ];
        
        const ul = document.createElement('ul');
        const currentPage = window.location.pathname.split('/').pop();

        sidebarLinks
            .filter(link => link.roles.includes(role)) // Filtra links pelo perfil
            .forEach(link => {
                const li = document.createElement('li');
                // Marca o link como 'active' se for a página atual
                const isActive = (link.href === currentPage) ? 'class="active"' : '';
                li.innerHTML = `<a href="${link.href}" ${isActive}><i data-feather="${link.icon}"></i> ${link.text}</a>`;
                ul.appendChild(li);
            });
        
        sidebarNav.innerHTML = ''; // Limpa o "Carregando..."
        sidebarNav.appendChild(ul);
        
        // Renderiza os ícones do Feather que acabamos de adicionar
        feather.replace();
    },
    
    /**
     * Anexa listeners de eventos (ex: menu mobile).
     */
    attachEventListeners: function() {
        // Listener para Sidebar Mobile
        if (this.elements.menuToggle && this.elements.sidebar) {
            this.elements.menuToggle.addEventListener('click', () => {
                this.elements.sidebar.classList.toggle('open');
            });
        }
    }
};

// --- Ponto de Entrada ---
document.addEventListener('DOMContentLoaded', () => {
    LayoutApp.init();
});