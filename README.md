# AzureFuncs
This Azure Function accepts the JSON Response from <a href="https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/">Microsoft Cognitive Services OCR</a> as an input.  It then parses the JSON and attempts to identify the text.  For example, when scanning a business card this function will attempt to identify the following:

1. Name
2. Company
3. Title
4. CityStateZip
5. Phone
6. Mobile
7. Fax
8. Email
9. Website
10. Facebook
11. Twitter
