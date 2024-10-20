# MarketPulseAPI

## Overview

MarketPulseAPI is a RESTful API service designed to provide price information for specific market assets, such as EUR/USD and GOOG. The service adheres to best practices in REST API design, ensuring a reliable and efficient interface for accessing market data.

## Running the Application
To run the application, navigate to the directory containing your `docker-compose.yml` file and use the following command:

```bash
docker-compose up
```

Follow the steps in the documentation to set up and run the MarketPulseAPI service in your local environment using Docker. Ensure that all prerequisites are met and that the necessary configuration settings are correctly applied.

For further details, please refer to the codebase and associated documentation.

### Version
1.0

### OpenAPI Specification
OAS3

### Swagger Documentation
HTTPS Access the API documentation at: [Swagger UI](https://localhost:32773/swagger/index.html) - https://localhost:32773/swagger/index.html

HTTP Access the API documentation at: [Swagger UI](http://localhost:32772/swagger/index.html) - http://localhost:32772/swagger/index.html

## Endpoints

### MarketAssets
- **GET** `/api/MarketAssets`
  - Get a list of supported market assets.
    
### AssetsPrices
- **GET** `/api/AssetsPrices/Historical`
  - Retrieve historical price information for specified market asset.
- **WSS** `api/assetsprices/ws/realtime?assetId={assetId}`
  - Subscribe for realtime price information for specified market asset.  
