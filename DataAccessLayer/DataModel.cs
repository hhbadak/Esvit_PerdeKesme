using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataModel
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private readonly string _connectionString = ConnectionStrings.ConStr; // ConnectionStrings sınıfındaki bağlantı dizesini kullanın.

        public DataModel()
        {
            con = new SqlConnection(_connectionString);
            cmd = con.CreateCommand();
        }

        #region Personal Metot
        public Employee personalLogin(string username, string password)
        {
            Employee model = new Employee();
            try
            {
                cmd.CommandText = "SELECT Kimlik FROM kullanici_liste WHERE kullanici_adi = @uName AND sifre = @password";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@uName", username);
                cmd.Parameters.AddWithValue("@password", password);
                con.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                if (id > 0)
                {
                    model = getPersonal(id);
                }
                return model;

            }
            catch
            {
                return null;
            }
            finally { con.Close(); }
        }

        public Employee getPersonal(int id)
        {
            try
            {
                Employee model = new Employee();
                cmd.CommandText = "SELECT Kimlik, kullanici_adi, sifre, ad_soyad, durum, pcAd, versiyon, KisaAd, Departman \r\nFROM kullanici_liste\r\nWHERE Kimlik = @id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.ID = Convert.ToByte(reader["Kimlik"]);
                    model.Username = reader.GetString(1);
                    model.Password = reader.GetString(2);
                    model.NameSurname = reader.GetString(3);
                    model.Status = reader.GetByte(4);
                    model.PcName = reader.GetString(5);
                    model.Version = reader.GetString(6);
                    model.ShortName = reader.GetString(7);
                    model.Department = reader.GetString(8);
                }
                return model;
            }
            catch
            {
                return null;
            }
            finally { con.Close(); }
        }
        #endregion

        public List<Products> getBarcodeQuality(string barcode)
        {
            List<Products> pr = new List<Products>();
            try
            {
                cmd.CommandText = "SELECT ID, Quality FROM Products WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", barcode);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products model = new Products
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            QualityID = Convert.ToInt32(reader["Quality"])
                        };
                        pr.Add(model);
                    }
                }
                return pr;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public bool updateProductAndStoning(DataAccessLayer.Kalite_PerdeKesme taharet)
        {
            try
            {
                // Önce Products tablosunu güncelle
                cmd.CommandText = "UPDATE Products SET Quality = @quality, Fault = 46 WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@quality", taharet.QualityID);
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // Ardından Kalite_PerdeKesme tablosuna kaydı ekle
                cmd.CommandText = "INSERT INTO Kalite_PerdeKesme(Barcode, QualityID, DateTime, QualityPersonalID) VALUES(@barcode, @quality, FORMAT(@date, 'yyyy-MM-dd HH:mm:ss'), @qpid)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);
                cmd.Parameters.AddWithValue("@quality", taharet.QualityID);
                cmd.Parameters.AddWithValue("@date", taharet.Datetime);
                cmd.Parameters.AddWithValue("@qpid", taharet.QualityPersonalID);

                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        #region Products Metot

        public DataAccessLayer.Kalite_PerdeKesme getProductDetails(string barcode)
        {
            try
            {
                cmd.CommandText = "SELECT Quality, FROM Products WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", barcode);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataAccessLayer.Kalite_PerdeKesme model = new DataAccessLayer.Kalite_PerdeKesme();
                if (reader.Read())
                {
                    model.QualityID = reader.GetByte(0);
                }
                return model;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public bool updateProductQuality(DataAccessLayer.Kalite_PerdeKesme taharet)
        {
            try
            {
                // Önce barkodun Products tablosunda var olup olmadığını kontrol et
                cmd.CommandText = "SELECT COUNT(*) FROM Products WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);
                con.Open();
                int productExists = Convert.ToInt32(cmd.ExecuteScalar());
                
                if (productExists == 0)
                {
                    con.Close();
                    throw new Exception($"Barkod '{taharet.Barcode}' Products tablosunda bulunamadı. Önce ürünü sisteme eklemelisiniz.");
                }
                
                // Barkod varsa güncelle
                cmd.CommandText = "UPDATE Products SET Quality = @quality, Fault = 46 WHERE Barcode=@barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@quality", taharet.QualityID);
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);
                int rowsAffected = cmd.ExecuteNonQuery();
                
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"updateProductQuality Hata: {ex.Message}");
                return false;
            }
            finally { con.Close(); }
        }

        #endregion

        #region Stoning Metot

        public bool isBarcodeExists(string barcode)
        {
            try
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Kalite_PerdeKesme WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", barcode);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool isProductExists(string barcode)
        {
            try
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Products WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", barcode);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public List<Kalite_PerdeKesme> logEntryListStoning(Kalite_PerdeKesme filter)
        {
            List<Kalite_PerdeKesme> rt = new List<Kalite_PerdeKesme>();
            try
            {
                cmd.CommandText = @"SELECT kt.ID, kt.Barcode, klt.tanim, rlt.Name, kt.DateTime, kl.ad_soyad  
FROM Kalite_PerdeKesme AS kt
JOIN kalite_liste AS klt ON klt.Kimlik = kt.QualityID
LEFT JOIN Kalite_TaharetBoruMontajHata AS rlt ON rlt.ID = kt.ResultID
JOIN kullanici_liste AS kl ON kl.Kimlik = kt.QualityPersonalID
WHERE CAST(kt.DateTime AS DATE) = CAST(GETDATE() AS DATE)";

                cmd.Parameters.Clear();

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Kalite_PerdeKesme model = new Kalite_PerdeKesme
                        {
                            ID = reader.GetInt32(0),
                            Barcode = reader.GetString(1),
                            Quality = reader.GetString(2),
                            Result = reader.IsDBNull(3) ? "-" : reader.GetString(3),
                            Datetime = reader.GetDateTime(4),
                            QualityPersonal = reader.GetString(5),
                        };
                        rt.Add(model);
                    }
                }
                return rt;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public bool createTaharetBoruMontaj(DataAccessLayer.Kalite_PerdeKesme taharet)
        {
            try
            {
                // 1. Önce kullanıcının var olup olmadığını kontrol et
                cmd.CommandText = "SELECT COUNT(*) FROM kullanici_liste WHERE Kimlik = @qpid";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@qpid", taharet.QualityPersonalID);
                con.Open();
                int userExists = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                
                if (userExists == 0)
                {
                    throw new Exception($"Kullanıcı ID {taharet.QualityPersonalID} veritabanında bulunamadı.");
                }

                // 2. Barkodun Products tablosunda var olup olmadığını kontrol et
                cmd.CommandText = "SELECT COUNT(*) FROM Products WHERE Barcode = @barcode";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);
                con.Open();
                int productExists = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                
                // Debug için barkod kontrolünü logla
                System.Diagnostics.Debug.WriteLine($"Barkod '{taharet.Barcode}' Products tablosunda kontrol edildi. Sonuç: {productExists}");
                
                if (productExists == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"HATA: Barkod '{taharet.Barcode}' Products tablosunda bulunamadı!");
                    throw new Exception($"Barkod '{taharet.Barcode}' Products tablosunda bulunamadı. Önce ürünü sisteme eklemelisiniz.");
                }
                
                // 3. Tüm kontroller başarılı, kayıt ekle
                cmd.CommandText = "INSERT INTO Kalite_PerdeKesme(Barcode, QualityID, DateTime, QualityPersonalID, ResultID) VALUES(@barcode, @quality, FORMAT(@date, 'yyyy-MM-dd HH:mm:ss'), @qpid, @resultid)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", taharet.Barcode);
                cmd.Parameters.AddWithValue("@quality", taharet.QualityID);
                cmd.Parameters.AddWithValue("@date", taharet.Datetime);
                cmd.Parameters.AddWithValue("@qpid", taharet.QualityPersonalID);
                cmd.Parameters.AddWithValue("@resultid", (object)taharet.ResultID ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                // Hata mesajını logla (isterseniz bir dosyaya yazabilirsiniz)
                System.Diagnostics.Debug.WriteLine($"createTaharetBoruMontaj Hata: {ex.Message}");
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

        #region Quality List Metot

        public List<QualityList> getQualityList()
        {
            List<QualityList> qualityList = new List<QualityList>();
            try
            {
                cmd.CommandText = "SELECT Kimlik, tanim FROM kalite_liste ORDER BY Kimlik";
                cmd.Parameters.Clear();
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QualityList model = new QualityList
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        qualityList.Add(model);
                    }
                }
                return qualityList;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

        #region Result List Metot

        public List<ResultList> getResultList()
        {
            List<ResultList> resultList = new List<ResultList>();
            try
            {
                cmd.CommandText = "SELECT ID, Name FROM Kalite_TaharetBoruMontajHata ORDER BY ID";
                cmd.Parameters.Clear();
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ResultList model = new ResultList
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        resultList.Add(model);
                    }
                }
                return resultList;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

        #region Result Metot

        public bool isThereResult(int id)
        {
            try
            {
                cmd.CommandText = "SELECT Kimlik FROM kalite_liste WHERE Kimlik = @id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally { con.Close(); }
        }

        #endregion
    }
}
