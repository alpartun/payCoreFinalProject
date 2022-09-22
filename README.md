# payCoreFinalProject

.Net/.Net Core final project organized by PayCore and Patika. In general, what is required in the project;

* User registration and login processes and necessary validations.

* Jwt token usage in login process.

* User can add or update category in API.

* User can add products according to required validations

* Other users can send offers to products

* If the product is not offerable to offers, the user cannot offer, user can only order.

* User can directly order the price of the product.

* If the product is sold, the status of the product must be updated and it must be made unofferable and unorderable.

* User should be able to see his offers.

* User should be able to see the offers he received.

* User can accept or reject the offer he receives.

* Notification should be made within the project via email.

* Sending email should be asynchronous.

* In addition, the user can see the products he ordered and sold.


## Table of Contents

1. [ Technologies and Packages ](#tech)
2. [ Swagger ](#swag)
- 2.1[ RegisterController ](#regs)
- 2.2 [ TokenController ](#tokn)
- 2.3 [ CategoryDetailsController ](#catg)
- 2.4 [ ProductDetailsController  ](#prod)
- 2.5 [ UserAccountDetailsController  ](#usac)
- 2.6 [ UserController  ](#user)

3. [ Postgre SQL ](#postg)
- 3.1 [ User Table ](#usertab)
- 3.2 [ Category Table ](#catgtab)
- 3.3 [ Product Table ](#prodtab)
- 3.4 [ Offer Table ](#offtab)
- 3.5 [ Email Table ](#emtab)
4. [ Etheral EmailBox ](#emb)
5. [ RabbitMQ  ](#rabmq)



<a name="tech"></a>

# 1.Technologies and Packages 

### .Net6

### Docker and RabbitMQ

```json
docker run --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

appsettings.json ->

```json
"RabbitMq":{
    "HostName":"localhost",
    "UserName": "guest",
    "Password" :"guest"
  }

```

### Fake SMTP Server

https://ethereal.email/

appsettings.json ->

```json
"Email":{
    "Port": 587,
    "RetryCount":5,
    "User": "abel.kilback@ethereal.email",
    "Pass": "qt3WdwZzRE2yeR9tsV",
    "From":"abel.kilback@ethereal.email",
    "Host":"smtp.ethereal.email"
  },

```
### JWT Token

appsettings.json ->

```json
"JwtConfig": {
    "Secret": "123ASD123TRT223RTA89JSLGKYMD23456109ASDASTURLENS12123ASDASCADXAP",
    "Issuer": "Final",
    "Audience": "Final",
    "AccessTokenExpiration": 1000
  },

```
### NuGet Packages
![1c1ca595-b66a-4b88-9797-26016661969e](https://user-images.githubusercontent.com/36279321/191614948-766ee852-89b6-43d6-89cf-80fbdd06ec9a.jpg)



<a name="swag"></a>

# 2.Swagger

<a name="regs"></a>

## 2.1 RegisterController

#### -> Register (POST)

![6f953003-628b-404d-b7bc-07ea1225a90d](https://user-images.githubusercontent.com/36279321/191622087-0544d096-f8b8-4dd3-814d-4d723c46b451.jpg)

![454b0c96-776d-49a3-abfe-4f0471b5da46](https://user-images.githubusercontent.com/36279321/191622938-827c9511-4766-48f2-ace7-82594543ddbe.jpg)



<a name="tokn"></a>

## 2.2 TokenController (Login)


#### -> Login (POST)

![e01889fd-092b-44c5-bacc-fa448bf164dc](https://user-images.githubusercontent.com/36279321/191622211-42fe2772-8bcb-4749-818b-eea63595dc20.jpg)

![fbeaa3dc-9fe6-49ba-b675-39e2db658917](https://user-images.githubusercontent.com/36279321/191622218-a1ec44b8-8721-4784-b759-f85d8e486511.jpg)

<a name="catg"></a>


# 2.3 CategoryDetailsController [Authorized]

![2be631c5-661a-402d-a819-6a164cb010a9](https://user-images.githubusercontent.com/36279321/191623666-111711cb-53d2-4da5-8e9b-9b3057411edf.jpg)


#### -> Category/All (GET)

![5725391c-c602-4209-bca5-e842e0494689](https://user-images.githubusercontent.com/36279321/191623175-ac8e2288-67e0-4061-a4ec-5e78eecaabae.jpg)


#### -> Category/{id} (GET)

![19112ec1-0066-4ff2-9a32-dac7d94cfa4c](https://user-images.githubusercontent.com/36279321/191629645-0b997060-8986-45ab-bd9f-3188027f3426.jpg)


#### -> Category/ (POST)

![dffad793-f1e9-44a8-acd2-770f41642cc3](https://user-images.githubusercontent.com/36279321/191623256-ec5d5238-a5ca-4dbd-8e18-dc4836aa2bbb.jpg)

#### -> Category (PUT)

![ef261c3b-c76c-46e0-973a-ec6249233509](https://user-images.githubusercontent.com/36279321/191629651-480f2918-bee5-480f-9a5d-8b14456b1503.jpg)


#### -> Category/{id} (DELETE)

![d7ee0c1e-704d-4418-b2c4-cf2629d394a8](https://user-images.githubusercontent.com/36279321/191629660-7da18abb-7497-4c3f-9e36-5639e850fdfd.jpg)

<a name="prod"></a>


# 2.4 ProductDetailsController [Authorized]

![522c90b4-5da0-4bf8-bbff-8394d59f72de](https://user-images.githubusercontent.com/36279321/191623682-c9fdb1b9-c140-4bb0-8bb7-1dd1516b9660.jpg)


#### -> Product/GetAll (GET)

![ca485724-f322-4e6f-bdd7-e20919e17d18](https://user-images.githubusercontent.com/36279321/191630622-5f976cc1-7029-46d2-a70a-af5f08f12b92.jpg)


#### -> Product/Get/{id} (GET)

![43377192-f9a3-4813-98e8-18698a84a8d2](https://user-images.githubusercontent.com/36279321/191630866-1fd22dd9-7037-452c-81d3-55c2c84f8aea.jpg)


#### -> Product/GetAllOfferaleProducts (GET)

![eab597ed-a84d-4865-86f6-1173e7b70383](https://user-images.githubusercontent.com/36279321/191623111-a9ee31ff-2a08-4118-9df1-e2c7c0469d78.jpg)


#### -> Product/OfferableByCategory/{id} (GET)

![aa92e284-ea2d-44c9-b8f8-fa99ed4a6c72](https://user-images.githubusercontent.com/36279321/191630760-24277f9f-fede-46be-a485-f998edb09f4d.jpg)

#### -> Product (POST)

![348376a5-46c9-4e2d-b1e6-d9c30c1726f3](https://user-images.githubusercontent.com/36279321/191623148-75af9fad-f74e-4e05-84f7-c13e9ef18edf.jpg)

![d0d4f364-7005-4044-b2c5-2e405a7e57f5](https://user-images.githubusercontent.com/36279321/191623216-76d06212-4a9f-40f5-8509-134b5c44fcc7.jpg)

#### -> Product/Order (POST)

![4a451a61-3d2e-4868-ba0c-49fd93e82508](https://user-images.githubusercontent.com/36279321/191630743-262f9be3-4ec5-49f8-9e70-b38e42c79feb.jpg)

#### -> Product/SendOffer (POST)

![5d1b428a-f4ef-495d-9008-f47c44c70cc8](https://user-images.githubusercontent.com/36279321/191623086-f78690ef-33d6-4f05-9426-0fe4f19e2c79.jpg)


#### -> Product/Offer/Accept (POST)

![d45136e2-a42a-4b0d-b43f-b74b072cb7eb](https://user-images.githubusercontent.com/36279321/191630721-5e22e6c0-b268-41a7-979e-4702278bcd5f.jpg)


#### -> Product/Offer/Reject (POST)

![9b02f0aa-019d-433a-9231-0c07f98f853f](https://user-images.githubusercontent.com/36279321/191630731-a8c7e510-3513-4bc3-a204-9de258bcc14b.jpg)

#### -> Product (PUT)

#### -> Product (DELETE)

![29703488-bc55-432c-a2a0-b8c4f9e746b8](https://user-images.githubusercontent.com/36279321/191631573-3abd6f51-f1d3-41c0-b9aa-9f516fb02d39.jpg)


<a name="usac"></a>


# 2.5 UserAccountDetailsController [Authorized]

![7c2dfb39-f706-4aeb-b52e-e99317457c01](https://user-images.githubusercontent.com/36279321/191623697-0a56e9c1-470e-4a6a-9c50-3575bcf29ca1.jpg)


#### -> User/Offer/{id} (GET)

![a35f0b00-3cde-494a-bf6e-51a35dffb0c8](https://user-images.githubusercontent.com/36279321/191630831-f8baf287-15e0-4990-a525-fe79e8ca0410.jpg)

#### -> User/AllOffers (GET)

![6b766832-37a0-4804-859b-511cb6822b3a](https://user-images.githubusercontent.com/36279321/191623041-247044a7-0d2d-4834-9222-227c42b70144.jpg)

![797396cd-e730-4063-8c8a-a022bae2a5a3](https://user-images.githubusercontent.com/36279321/191630812-d8d8cded-f4cf-42b6-9617-f93a6c385095.jpg)

#### -> User/AllOfferedProducts (GET)

![de0e0e27-70ff-433b-9875-01cd7ea5f50e](https://user-images.githubusercontent.com/36279321/191630685-a8a982ba-0c72-449d-ac7b-2fc8c1ed5d85.jpg)

#### -> User/AllOrders (GET)

![5044e035-561f-409c-aacf-5972b47d91e8](https://user-images.githubusercontent.com/36279321/191630655-5d669391-75fc-4eee-ac9f-9a1581f8c250.jpg)


#### -> User/AllSoldProducts (GET)

![ad07cf9a-f12a-4848-8518-01b9768f005c](https://user-images.githubusercontent.com/36279321/191630668-227a111b-2c1b-4b76-8e45-6dbc52c52815.jpg)

#### -> User/Offer (PUT)

![c8f0eb7f-4bf1-4d3d-9dc6-9a11c048917f](https://user-images.githubusercontent.com/36279321/191631541-b3b677ec-0054-40bb-ac5e-f59459a5612d.jpg)


#### -> User/Offer (DELETE)
![30b4eb3f-54e4-4a62-b0a6-efa52b293ed6](https://user-images.githubusercontent.com/36279321/191631551-3a4c0d90-0f0a-4e3d-a172-8f3915f87532.jpg)


<a name="user"></a>

# 2.6 UserController [NonController]



<a name="postg"></a>

# 3.Postgre SQL

<a name="usertab"></a>

## 3.1 User Table 

![075be5fa-4f84-4e70-86f8-586de57d55a8](https://user-images.githubusercontent.com/36279321/191623633-4a262523-9dbe-4c47-90ad-4ca1bce6ed08.jpg)

<a name="catgtab"></a>

## 3.2 Category Table 

![401a7597-2d2d-4ca1-abcf-592a3af41926](https://user-images.githubusercontent.com/36279321/191622972-e4499f2f-75c7-450a-ab5a-0a26dd13e270.jpg)



<a name="prodtab"></a>

## 3.2 Product Table 

![7b014c93-a984-467c-9407-479476b7ba74](https://user-images.githubusercontent.com/36279321/191622984-accb3142-e233-4e07-93f8-713979c6e12f.jpg)

<a name="offtab"></a>

## 3.2 Offer Table 

![df45cccc-8081-4a1f-9579-2517c5f9f6c4](https://user-images.githubusercontent.com/36279321/191622997-eadb67b9-6a2f-4043-9e0f-c10f0e70d00c.jpg)


<a name="emtab"></a>

## 3.2 Email Table 

![b2401d47-49cf-46a7-a69d-ef21e41a58f0](https://user-images.githubusercontent.com/36279321/191622859-905a6ba8-4318-4330-899e-d25d5876ab4a.jpg)


<a name="emb"></a>

## 4.Etheral EMailBox

![f6e6f4ab-e44e-48ff-ab75-e0bc936d4e6f](https://user-images.githubusercontent.com/36279321/191622158-b0f51359-cbbf-4245-93f0-ac3d79a0a272.jpg)

![5a12c3da-8bad-4292-b627-c518cb1e99fe](https://user-images.githubusercontent.com/36279321/191622170-e2c2356b-3f3a-4605-ba70-e5d13ef65b22.jpg)

<a name="rabmq"></a>

## 5.RabbitMQ 

![5ca4ff10-82e2-4617-974f-ca2be6c9bb42](https://user-images.githubusercontent.com/36279321/191622844-186cce90-b5ff-4a51-b3bb-6e117607b685.jpg)

