using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

private void Form1_Load(object sender, EventArgs e)
  {
            try
            {
                DataTable oTable = new DataTable();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VendasSQL"].ConnectionString))
                {
                    string sql = "Select * from Itens";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader oDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    oTable.Load(oDataReader);
                    gdvItens.DataSource = oTable;
                    formataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
 }


private void formataGridView()
 {
            var grade = gdvItens;
            grade.AutoGenerateColumns = false;
            grade.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            grade.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            //altera a cor das linhas alternadas no grid
            grade.RowsDefaultCellStyle.BackColor = Color.White;
            grade.AlternatingRowsDefaultCellStyle.BackColor = Color.Cyan;
            //altera o nome das colunas
            grade.Columns[0].HeaderText = "Id";
            grade.Columns[1].HeaderText = "Descrição";
            grade.Columns[2].HeaderText = "Quantidade";
            grade.Columns[3].HeaderText = "Preco";
            grade.Columns[4].HeaderText = "Total";
            grade.Columns[0].Width = 70;
            grade.Columns[1].Width = 150;
            //formata as colunas valor, vencimento e pagamento
            grade.Columns[3].DefaultCellStyle.Format = "c";
            grade.Columns[4].DefaultCellStyle.Format = "c";
            //seleciona a linha inteira
            grade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //não permite seleção de multiplas linhas    
            grade.MultiSelect = false;
            // exibe nulos formatados
            //grade.DefaultCellStyle.NullValue = " - ";
            //permite que o texto maior que célula não seja truncado
            grade.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //define o alinhamento à direita
            grade.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grade.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
  }

  private void btnSalvar_Click(object sender, EventArgs e)
 {
            SalvarDados();
 }

 public void SalvarDados()
 {
            try
            {
                if (gdvItens.Rows.Count > 1)
                {
                    for (int i = 0; i <= gdvItens.Rows.Count - 1; i++)
                    {
                         int col1 = Convert.ToInt32(gdvItens.Rows[i].Cells[0].Value); //id
                         string col2 = gdvItens.Rows[i].Cells[1].Value.ToString(); //Descricao 
                         int col3 = Convert.ToInt32(gdvItens.Rows[i].Cells[2].Value); //Quantidade
                         decimal col4 = Convert.ToDecimal(gdvItens.Rows[i].Cells[3].Value); //Preco
                         decimal col5 = Convert.ToDecimal(gdvItens.Rows[i].Cells[4].Value); //Total
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VendasSQL"].ConnectionString))
                        {
                            string insert = "INSERT INTO Itens(Id,Descricao,Quantidade,Preco,Total) VALUES (@Codigo,@Descricao,@Quantidade,@Preco,@Total)";
                            con.Open();
                            SqlCommand cmd = new SqlCommand(insert, con);
                            cmd.Parameters.AddWithValue("@Codigo", col1);
                            cmd.Parameters.AddWithValue("@Descricao", col2);
                            cmd.Parameters.AddWithValue("@Quantidade", col3);
                            cmd.Parameters.AddWithValue("@Preco", col4);
                            cmd.Parameters.AddWithValue("@Total", col5);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
          MessageBox.Show("Dados incluídos com sucesso !!", "Inclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
 }

 private void gdvItens_CellEndEdit(object sender, DataGridViewCellEventArgs e)
 {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    decimal cell1 = Convert.ToDecimal(gdvItens.CurrentRow.Cells[2].Value);
                    decimal cell2 = Convert.ToDecimal(gdvItens.CurrentRow.Cells[3].Value);
                    if (cell1.ToString() != "" && cell2.ToString() != "")
                    {
                        gdvItens.CurrentRow.Cells[4].Value = cell1 * cell2;
                    }
                }
                decimal valorTotal = 0;
                string valor = "";
                if (gdvItens.CurrentRow.Cells[4].Value != null)
                {
                    valor = gdvItens.CurrentRow.Cells[4].Value.ToString();
                    if (!valor.Equals(""))
                    {
                        for (int i = 0; i <= gdvItens.RowCount - 1; i++)
                        {
                            if (gdvItens.Rows[i].Cells[4].Value != null)
                                valorTotal += Convert.ToDecimal(gdvItens.Rows[i].Cells[4].Value);
                        }
                        if (valorTotal == 0)
                        {
                            MessageBox.Show("Nenhum registro encontrado");
                        }
                        txtTotal.Text = valorTotal.ToString("C");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        decimal cell1 = Convert.ToDecimal(gdvItens.CurrentRow.Cells[2].Value);
decimal cell2 = Convert.ToDecimal(gdvItens.CurrentRow.Cells[3].Value);
if (cell1.ToString() != "" && cell2.ToString() != "")
{
         gdvItens.CurrentRow.Cells[4].Value = cell1 * cell2;
}

// Comandobanco de dados: Criando uma tabela.


CREATE PROCEDURE dbo.buscaCliCPF
         @CPF varchar(15)
  AS
         Select cpf, nome, endereco, telefone
         from clientes
         where CPF=@CPF
ALTER PROCEDURE dbo.buscaCliNome
         @nome varchar(50)
  AS
         select cpf, nome, endereco, telefone
         from clientes
         where nome like @nome +'%'
         CREATE PROCEDURE dbo.buscaTodos
  AS
         Select cpf, nome, endereco, telefone
         from clientes
         CREATE PROCEDURE dbo.ExcluirCliente
         @CPF varchar(15)
  AS
         delete from clientes where cpf = @cpf
         CREATE PROCEDURE dbo.inserir_alterar_Cliente
         @CPF varchar(15),
         @nome varchar(50),
         @endereco varchar (100),
         @telefone varchar(15),
         @flag int
  AS
         if (@flag = 1)
         begin
               insert into Clientes(cpf,nome,endereco,telefone)
               values(@CPF,@nome,@endereco,@telefone)
         end
         else begin
               update clientes 
               set nome = @nome, endereco = @endereco, telefone = @telefone
               where cpf = @cpf
         end

         Classe para validação do CPF.

using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
   
  namespace CSharpComSQLServer
  {
      public class Validacoes
      {
          public static bool validarCPF(string cpf)
          {
              string valor = cpf.Replace(".", "");
              valor = valor.Replace("-", "");
   
              if (valor.Length != 11)
                  return false;
   
              bool igual = true;
              for (int i = 1; i < 11 && igual; i++)
                  if (valor[i] != valor[0])
                      igual = false;
   
              if (igual || valor == "12345678909")
                  return false;
   
              int[] numeros = new int[11];
              for (int i = 0; i < 11; i++)
                  numeros[i] = int.Parse(valor[i].ToString());
   
              int soma = 0;
              for (int i = 0; i < 9; i++)
                  soma += (10 - i) * numeros[i];
   
              int resultado = soma % 11;
              if (resultado == 1 || resultado == 0)
              {
                  if (numeros[9] != 0)
                      return false;
              }
              else if (numeros[9] != 11 - resultado)
                  return false;
              soma = 0;
   
              for (int i = 0; i < 10; i++)
                  soma += (11 - i) * numeros[i];
   
              resultado = soma % 11;
              if (resultado == 1 || resultado == 0)
              {
                  if (numeros[10] != 0)
                      return false;
              }
              else
                  if (numeros[10] != 11 - resultado)
                      return false;
              return true;
          }
      }
  }  
  
           
