{
  /*نظرات و تجربه‌های کاربران*/
}

import { motion } from "framer-motion";
import Container from "./ui/Container";

export default function Testimonials() {
  const testimonials = [
    {
      name: "علی احمدی",
      company: "مدیر فروشگاه آریا",
      text: "تیم رشدینو با طراحی سایت و سئو باعث شد فروش آنلاین ما چند برابر شود.",
    },
    {
      name: "سارا محمدی",
      company: "برند نوین",
      text: "استراتژی دیجیتال مارکتینگ رشدینو دقیقاً چیزی بود که کسب‌وکار ما نیاز داشت.",
    },
    {
      name: "محمد رضایی",
      company: "استارتاپ آموزشی",
      text: "از همکاری با رشدینو راضی هستیم؛ تیمی حرفه‌ای و نتیجه‌محور.",
    },
  ];

  return (
    <motion.section
      initial={{
        opacity: 0,
        y: 50,
      }}
      whileInView={{
        opacity: 1,
        y: 0,
      }}
      viewport={{
        once: true,
      }}
      transition={{
        duration: 0.6,
      }}
      className="
            py-24
            bg-gray-50
            "
    >
      <Container>
        <div
          className="
                text-center
                "
        >
          <h2
            className="
                    text-4xl
                    font-black
                    "
          >
            مشتریان درباره ما چه می‌گویند؟
          </h2>

          <p
            className="
                    mt-5
                    text-gray-600
                    "
          >
            تجربه همکاری برندهایی که به ما اعتماد کرده‌اند
          </p>
        </div>

        <div
          className="
                mt-14
                grid
                md:grid-cols-3
                gap-8
                "
        >
          {testimonials.map((item, index) => (
            <motion.div
              key={item.name}
              initial={{
                opacity: 0,
                y: 40,
              }}
              whileInView={{
                opacity: 1,
                y: 0,
              }}
              viewport={{
                once: true,
              }}
              transition={{
                delay: index * 0.15,
                duration: 0.5,
              }}
              whileHover={{
                y: -6,
                scale: 1.01,
              }}
              className="
                        bg-white
                        rounded-3xl
                        p-8
                        border
                        hover:shadow-xl
                        transition
                        "
            >
              <div
                className="
                            text-yellow-400
                            text-xl
                            "
              >
                ★★★★★
              </div>

              <p
                className="
                            mt-6
                            text-gray-600
                            leading-8
                            "
              >
                "{item.text}"
              </p>

              <div
                className="
                            mt-8
                            flex
                            items-center
                            gap-4
                            "
              >
                <div
                  className="
                                w-12
                                h-12
                                rounded-full
                                bg-purple-600
                                text-white
                                flex
                                items-center
                                justify-center
                                font-bold
                                "
                >
                  {item.name[0]}
                </div>

                <div>
                  <h4
                    className="
                                    font-bold
                                    "
                  >
                    {item.name}
                  </h4>

                  <p
                    className="
                                    text-sm
                                    text-gray-500
                                    "
                  >
                    {item.company}
                  </p>
                </div>
              </div>
            </motion.div>
          ))}
        </div>
      </Container>
    </motion.section>
  );
}
