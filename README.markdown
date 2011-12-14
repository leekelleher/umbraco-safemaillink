# SafeMailLink for Umbraco

Prevents spambots from harvesting email addresses from your webpages.

This package dynamically encodes all the _mailto:_ links found on your rendered webpages at runtime.

Any _mailto:_ links with pre-populated parameters for subject and body fields are also encoded. Email addresses used as text for the email hyperlink are protected as well.