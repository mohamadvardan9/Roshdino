import Container from "./ui/Container";

export default function Footer() {
  const services = [
    "طراحی سایت",
    "سئو",
    "تولید محتوا",
    "برندینگ",
    "بازاریابی دیجیتال",
  ];

  const links = ["درباره ما", "نمونه کارها", "وبلاگ", "تماس با ما"];

  return (
    <footer
      className="
            bg-gray-950
            text-white
            pt-12
            pb-6
            "
    >
      {/* pt-20
        pb-8 */}

      <Container>
        <div
          className="
                grid
                md:grid-cols-4
                gap-10
                "
        >
          {/* Brand */}

          <div>
            <h2
              className="
                        text-3xl
                        font-black
                        text-purple-400
                        "
            >
              رشدینو
            </h2>

            <p
              className="
                        mt-5
                        text-gray-400
                        leading-8
                        "
            >
              راهکارهای دیجیتال برای رشد هوشمندانه کسب‌وکارها.
            </p>
          </div>

          {/* Services */}

          <div>
            <h3
              className="
                        text-lg
                        font-bold
                        mb-5
                        "
            >
              خدمات
            </h3>

            <ul
              className="
                        space-y-3
                        text-gray-400
                        "
            >
              {services.map((service) => (
                <li
                  key={service}
                  className="
                                hover:text-white
                                transition
                                cursor-pointer
                                "
                >
                  {service}
                </li>
              ))}
            </ul>
          </div>

          {/* Links */}

          <div>
            <h3
              className="
                        text-lg
                        font-bold
                        mb-5
                        "
            >
              دسترسی سریع
            </h3>

            <ul
              className="
                        space-y-3
                        text-gray-400
                        "
            >
              {links.map((link) => (
                <li
                  key={link}
                  className="
                                hover:text-white
                                transition
                                cursor-pointer
                                "
                >
                  {link}
                </li>
              ))}
            </ul>
          </div>

          {/* Contact */}

          <div>
            <h3
              className="
                        text-lg
                        font-bold
                        mb-5
                        "
            >
              ارتباط با ما
            </h3>

            <p
              className="
                        text-gray-400
                        leading-8
                        "
            >
              ایمیل:
              <br />
              info@rozhdino.ir
              <br />
              <br />
              تلفن:
              <br />
              ۰۹۱۲۱۲۳۴۵۶۷
            </p>
          </div>
        </div>

        <div
          className="
                mt-16
                pt-8
                border-t
                border-white/10
                text-center
                text-gray-500
                text-sm
                "
        >
          © 2026 رشدینو - تمامی حقوق محفوظ است.
        </div>
      </Container>
    </footer>
  );
}
