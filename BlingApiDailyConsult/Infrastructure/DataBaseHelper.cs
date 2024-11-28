using BlingApiDailyConsult.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace BlingApiDailyConsult.Infrastructure
{
    public class DataBaseHelper
    {
        private readonly string? _connectionString;

        public DataBaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Insere ou atualiza um token no banco de dados.
        /// </summary>

        public void InsertOrUpdateToken(TokenInfo tokenInfo)
        {
            if (tokenInfo == null)
            {
                throw new ArgumentNullException(nameof(tokenInfo), "O objeto TokenInfo não pode ser nulo.");
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"
                    INSERT INTO tokens
                        (access_token, expires_in, token_type, scope, refresh_token, created_at)
                    VALUES
                        (@access_token, @expires_in, @token_type, @scope, @refresh_token, @created_at)
                    ON DUPLICATE KEY UPDATE
                        access_token = VALUES(access_token),                        
                        expires_in = VALUES(expires_in),
                        token_type = VALUES(token_type),
                        scope = VALUES(scope),
                        refresh_token = VALUES(refresh_token),
                        created_at = VALUES(created_at);";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@access_token", tokenInfo.AccessToken);
                        cmd.Parameters.AddWithValue("@expires_in", tokenInfo.ExpiresIn);
                        cmd.Parameters.AddWithValue("@token_type", tokenInfo.TokenType);
                        cmd.Parameters.AddWithValue("@scope", tokenInfo.Scope);
                        cmd.Parameters.AddWithValue("@refresh_token", tokenInfo.RefreshToken);
                        cmd.Parameters.AddWithValue("@created_at", tokenInfo.DatetimeNowUtc);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"SQL Insert or Update ERROR: + {ex.Message}");
            }
        }

        public TokenInfo GetTokenFromDatabase()
        {
            TokenInfo? tokenInfo = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"
                        SELECT Access_Token, Expires_In, Token_Type, Scope, Refresh_Token, Created_at
                        FROM tokens
                        WHERE created_at = (SELECT MAX(created_at) FROM tokens);";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tokenInfo = new TokenInfo
                                {
                                    AccessToken = reader.GetString(reader.GetOrdinal("Access_Token")),
                                    ExpiresIn = reader.GetInt32(reader.GetOrdinal("Expires_In")),
                                    TokenType = reader.GetString(reader.GetOrdinal("Token_Type")),
                                    Scope = reader.GetString(reader.GetOrdinal("Scope")),
                                    RefreshToken = reader.GetString(reader.GetOrdinal("Refresh_Token")),
                                    DatetimeNowUtc = reader.GetDateTime(reader.GetOrdinal("Created_at"))
                                };
                            }
                            if (tokenInfo == null)
                            {
                                throw new InvalidOperationException("Nenhum token foi encontrado na base de dados. Por favor, insira um token inicial usando o fluxo de autenticação.");
                            }
                        }
                    }
                }

            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao buscar token do banco de dados: {ex.Message}", ex);
            }

            return tokenInfo;
        }

        // Insere ou atualiza uma lista de pedidos no banco de dados
        public void SavePedidos(IEnumerable<Pedido> pedidos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var pedido in pedidos)
                    {
                        InsertOrUpdatePedido(pedido, conn);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar o pedido no banco de dados: {ex.Message}", ex);
            }
        }

        // Método para inserir um pedido no banco de dados
        private void InsertOrUpdatePedido(Pedido pedido, MySqlConnection conn)
        {
            // Comando SQL para inserir os dados do pedido na tabela de pedidos do banco
            string sql = @"
                INSERT INTO pedidos 
                    (id, numero, data, total_produtos, status_id, status_valor, cliente_id, cliente_nome) 
                VALUES 
                    (@id, @numero, @data, @total_produtos, @status_id, @status_valor, @cliente_id, @cliente_nome)
                ON DUPLICATE KEY UPDATE 
                    numero = VALUES(numero),
                    data = VALUES(data),
                    total_produtos = VALUES(total_produtos),
                    status_id = VALUES(status_id),
                    status_valor = VALUES(status_valor),
                    cliente_id = VALUES(cliente_id),
                    cliente_nome = VALUES(cliente_nome);";

            // Cria um comando SQL com o comando definido acima e a conexão com o banco de dados
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", pedido.Id);
                cmd.Parameters.AddWithValue("@numero", pedido.Numero);
                cmd.Parameters.AddWithValue("@data", pedido.Data);
                cmd.Parameters.AddWithValue("@total_produtos", pedido.TotalProdutos);
                cmd.Parameters.AddWithValue("@status_id", pedido?.Situacao?.Id);
                cmd.Parameters.AddWithValue("@status_valor", pedido?.Situacao?.Valor);
                cmd.Parameters.AddWithValue("@cliente_id", pedido?.Contato?.Id);
                cmd.Parameters.AddWithValue("@cliente_nome", pedido?.Contato?.Nome);

                // Executa o comando no banco de dados
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveProdutos(IEnumerable<Produto> produtos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var produto in produtos)
                    {
                        InsertOrUpdateProduto(produto, conn);

                        // Apagar depois
                        Console.WriteLine();
                        Console.WriteLine(this + $"Produto: {produto.Id} inserido com sucesso no banco de dados");
                        Console.WriteLine();
                    }                   
                }                
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar o produto no banco de dados: {ex.Message}", ex);
            }
        }

        private void InsertOrUpdateProduto(Produto produto, MySqlConnection conn)
        {
            // Comando SQL para inserir os dados do produto na tabela de produtos do banco de dados
            string sql = @"
                INSERT INTO produtos 
                    (id, nome, codigo, preco, preco_custo, saldo_total_estoque, tipo, situacao, formato) 
                VALUES 
                    (@id, @nome, @codigo, @preco, @preco_custo, @saldo_total_estoque, @tipo, @situacao, @formato)
                ON DUPLICATE KEY UPDATE 
                    nome = VALUES(nome),
                    codigo = VALUES(codigo),
                    preco = VALUES(preco),
                    preco_custo = VALUES(preco_custo),
                    saldo_total_estoque = VALUES(saldo_total_estoque),
                    tipo = VALUES(tipo),
                    situacao = VALUES(situacao),
                    formato = VALUES(formato);";

            // Cria um comando SQL com o comando definido acima e a conexão com o banco
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", produto.Id);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@codigo", produto.Codigo);
                cmd.Parameters.AddWithValue("@preco", produto.Preco);
                cmd.Parameters.AddWithValue("@preco_custo", produto.PrecoCusto);
                cmd.Parameters.AddWithValue("@saldo_total_estoque", produto.Estoque?.SaldoTotal);
                cmd.Parameters.AddWithValue("@tipo", produto.Tipo);
                cmd.Parameters.AddWithValue("@situacao", produto.Situacao);
                cmd.Parameters.AddWithValue("@formato", produto.Formato);

                // Executa o comando no banco de dados
                cmd.ExecuteNonQuery();
            }
        }

        public List<string> GetPedidoIdFromDataBase()
        {
            List<string> pedidosIdList = new List<string>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"SELECT id
                            FROM pedidos
                            WHERE id NOT IN (SELECT pedido_id FROM itens_do_pedido);";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pedidosIdList.Add(reader["Id"].ToString());
                        }
                    }
                }
            }
            return pedidosIdList;
        }

        public void SavePedidoItens(Dictionary<string, List<Item>> pedidos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var pedido in pedidos)
                    {
                        // Começa uma transação para cada chave (pedido)
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in pedido.Value)
                                {
                                    // Apenas para debugar
                                    Console.WriteLine(this + $" Pedido: {pedido.Key}, Produto: {item?.Produto?.Id}");

                                    // Inserção ou atualização do item
                                    InsertOrUpdatePedidoitem(pedido.Key, item, conn, transaction);
                                }

                                // Se não houver erro, realiza o commit da transação para esta chave (pedido)
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Em caso de erro, faz o rollback apenas da transação atual (para este pedido)
                                transaction.Rollback();
                                Console.WriteLine($"Erro ao processar pedido {pedido.Key}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar os itens do pedido no banco de dados: {ex.Message}", ex);
            }
            // Apagar depois
            Console.WriteLine("Pedidos Inseridos com sucesso!");
        }

        private void InsertOrUpdatePedidoitem(string key, Item? item, MySqlConnection conn, MySqlTransaction transaction)
        {
            try
            {
                Task task = VerifyAndInsertProduto(item.Produto.Id, conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine($"Detalhes: {ex.StackTrace}");
            }

            // Comando SQL para inserir os dados dos itens do pedido na tabela de pedidos do banco
            string sql = @"INSERT INTO itens_do_pedido
                            (pedido_id, produto_id, nome_produto, quantidade, unidade_produto, preco_produto)
                        VALUES
                            (@pedido_id, @produto_id, @nome_produto, @quantidade, @unidade_produto, @preco_produto)
                        ON DUPLICATE KEY UPDATE
                            produto_id = VALUES(produto_id),
                            nome_produto = VALUES(nome_produto),
                            quantidade = VALUES(quantidade),
                            unidade_produto = VALUES(unidade_produto),
                            preco_produto = VALUES(preco_produto);";
            // Cria um comando SQL com o comando definido acima e a conexão com o banco de dados
            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@pedido_id", key);
                cmd.Parameters.AddWithValue("@produto_id", item?.Produto?.Id);
                cmd.Parameters.AddWithValue("@nome_produto", item?.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", item?.Quantidade);
                cmd.Parameters.AddWithValue("@unidade_produto", item?.Unidade);
                cmd.Parameters.AddWithValue("@preco_produto", item?.Valor);

                // Executa o comando no banco de dados
                cmd.ExecuteNonQuery();
            }
        }

        private static bool ProdutoExist(long ProdutoId, MySqlConnection conn)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM produtos WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", ProdutoId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar a existência do produto: {ex.Message}");
                return false; // Considera falso se ocorrer um erro
            }
        }

        private async Task VerifyAndInsertProduto(long produtoId, MySqlConnection conn)
        {
            try
            {
                if (!ProdutoExist(produtoId, conn))
                {
                    TokenManager tokenManager = new TokenManager(this);

                    BlingSingleProdutoFetcher singleProdutoFetcher = new BlingSingleProdutoFetcher(tokenManager);

                    var produto = await singleProdutoFetcher.GetSingleProduto(produtoId);

                    if (produto == null)
                    {
                        Console.WriteLine(this + $"Produto {produtoId} não encontrado na API!");
                    }

                    Console.WriteLine(this + $"Produto {produtoId} recuperado com sucesso.");

                    List<Produto>? produtos = new List<Produto>();
                    produtos.Add(produto);
                    SaveProdutos( produtos );
                }
                else
                {
                    Console.WriteLine(this + $" Produto {produtoId} já existe no banco de dados.");
                }
            }
            catch(Exception ex)
            {
                // Caso ocorra algum erro ao recuperar ou inserir o produto
                Console.WriteLine($"Erro ao verificar ou inserir produto {produtoId}: {ex.Message}");
                // lançar ou re-throw a exceção
            }
        }

    }
}
